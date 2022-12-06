using Extensions.DeviceDetector;
using Extensions.Password;
using Google.Authenticator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using XMessenger.Application.Dtos;
using XMessenger.Application.Dtos.Identity;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers;
using XMessenger.Helpers.Identity;
using XMessenger.Helpers.Services;
using XMessenger.Infrastructure.Data.EntityFramework.Context;

namespace XMessenger.Application.Services
{
    public interface IAuthService
    {
        Task<Result<bool>> RegisterAsync(RegisterDto registerDto);
        Task<Result<bool>> ConfirmAsync(string code, int userId);
        Task<Result<bool>> SendConfirmAsync(int userId);
        Task<Result<JwtTokenDto>> LoginByPasswordAsync(LoginDto loginDto);
        Task<Result<JwtTokenDto>> LoginByMFAAsync(LoginMFADto mfaDto);

        Task<Result<MFADto>> EnableMFAAsync(string code = null);
        Task<Result<bool>> DisableMFAAsync(string code);
    }

    public class AuthService : IAuthService
    {
        private readonly IdentityContext _db;
        private readonly IDetector _detector;
        private readonly IIdentityService _identityService;
        private readonly ILocationService _locationService;
        private readonly ITokenService _tokenService;
        public AuthService(IdentityContext db, IDetector detector, IIdentityService identityService, ILocationService locationService, ITokenService tokenService)
        {
            _db = db;
            _detector = detector;
            _identityService = identityService;
            _locationService = locationService;
            _tokenService = tokenService;
        }

        public async Task<Result<bool>> ConfirmAsync(string code, int userId)
        {
            var confirmRequest = await _db.Confirms.AsNoTracking().Include(s => s.User).FirstOrDefaultAsync(s => s.Code == code && s.UserId == userId);
            if (confirmRequest == null)
                return Result<bool>.NotFound("Code not found");

            if(confirmRequest.IsActivated)
                return Result<bool>.Error("Code already activated");

            var now = DateTime.Now;

            if (!confirmRequest.IsActualyRequest(now))
                return Result<bool>.Error("Code was expired. Please request new");

            confirmRequest.IsActivated = true;
            confirmRequest.ActivetedAt = now;

            var user = confirmRequest.User;

            user.IsConfirmed = true;

            _db.Confirms.Update(confirmRequest);
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return Result<bool>.Success();
        }

        public async Task<Result<bool>> DisableMFAAsync(string code)
        {
            var userId = _identityService.GetUserId();

            var userForDisableMFA = await _db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);

            if (userForDisableMFA == null)
                return Result<bool>.NotFound("User not found");

            if (!userForDisableMFA.MFA)
                return Result<bool>.NotFound("MFA already diactivated");

            var twoFactor = new TwoFactorAuthenticator();

            if (!twoFactor.ValidateTwoFactorPIN(userForDisableMFA.MFASecretKey, code))
                return Result<bool>.Error("Code is incorrect");


            userForDisableMFA.MFA = false;
            userForDisableMFA.MFASecretKey = null;

            var activeMFA = await _db.MFAs.FirstOrDefaultAsync(s => s.UserId == userId && s.IsActivated);
            if (activeMFA == null)
                return Result<bool>.Error("Some error, please contact support");

            activeMFA.Diactived = DateTime.Now;
            activeMFA.DiactivedBySessionId = _identityService.GetCurrentSessionId();

            _db.Users.Update(userForDisableMFA);
            _db.MFAs.UpdateRange(activeMFA);
            await _db.SaveChangesAsync();

            return Result<bool>.Success();
        }

        public async Task<Result<MFADto>> EnableMFAAsync(string code = null)
        {
            var userId = _identityService.GetUserId();

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userId);

            if (user == null)
                return Result<MFADto>.NotFound("User not found");

            if (code == null)
            {
                var existMFA = await _db.MFAs.FirstOrDefaultAsync(s => s.UserId == userId && !s.IsActivated);
                if (existMFA == null)
                {
                    var secretKey = Guid.NewGuid().ToString("N");
                    var twoFactor = new TwoFactorAuthenticator();
                    var setupInfo = twoFactor.GenerateSetupCode("XMessenger", user.Login, secretKey, false, 3);

                    user.MFASecretKey = secretKey;
                    user.MFA = false;

                    var newMFA = new MFA
                    {
                        UserId = userId,
                        EntryCode = setupInfo.ManualEntryKey,
                        QrCodeBase64 = setupInfo.QrCodeSetupImageUrl,
                        Secret = secretKey,
                        IsActivated = false,
                        Activated = null,
                        ActivatedBySessionId = null
                    };

                    _db.Users.Update(user);
                    await _db.MFAs.AddAsync(newMFA);
                    await _db.SaveChangesAsync();

                    return Result<MFADto>.SuccessWithData(new MFADto
                    {
                        QrCodeImage = setupInfo.QrCodeSetupImageUrl,
                        ManualEntryKey = setupInfo.ManualEntryKey,
                    });
                }
                else
                {
                    return Result<MFADto>.SuccessWithData(new MFADto
                    {
                        QrCodeImage = existMFA.QrCodeBase64,
                        ManualEntryKey = existMFA.EntryCode
                    });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(user.MFASecretKey))
                    return Result<MFADto>.Error("Unable to activate MFA");

                var mfaToActivate = await _db.MFAs.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId && !s.IsActivated);

                if (mfaToActivate == null)
                    return Result<MFADto>.Error("Unable to activate MFA");

                if (mfaToActivate.Secret != user.MFASecretKey)
                    return Result<MFADto>.Error("Please write to support as soon as possible");

                var twoFactor = new TwoFactorAuthenticator();

                if (!twoFactor.ValidateTwoFactorPIN(mfaToActivate.Secret, code))
                    return Result<MFADto>.Error("Code is incorrect");

                user.MFA = true;
                await _db.SaveChangesAsync();

                mfaToActivate.IsActivated = true;
                mfaToActivate.Activated = DateTime.Now;
                mfaToActivate.ActivatedBySessionId = _identityService.GetCurrentSessionId();
                mfaToActivate.RestoreCodes = Generator.GetRestoreCodes();

                _db.Users.Update(user);
                _db.MFAs.Update(mfaToActivate);
                await _db.SaveChangesAsync();

                return Result<MFADto>.SuccessWithData(new MFADto
                {
                    RestoreCodes = mfaToActivate.RestoreCodes,
                });
            }
        }

        public async Task<Result<JwtTokenDto>> LoginByMFAAsync(LoginMFADto mfaDto)
        {
            var sessionId = Guid.Parse(mfaDto.SessionId);

            var session = await _db.Sessions.AsNoTracking().Include(s => s.User).FirstOrDefaultAsync(s => s.Id == sessionId);

            if (session == null)
                return Result<JwtTokenDto>.NotFound($"Session with ID: {sessionId} not found");

            var secretKey = session.User.MFASecretKey;

            var twoFactor = new TwoFactorAuthenticator();

            var result = twoFactor.ValidateTwoFactorPIN(secretKey, mfaDto.Code);

            if (!result)
                return Result<JwtTokenDto>.Error("Code is incorrect");

            var jwtToken = await _tokenService.GetUserTokenAsync(new UserTokenDto
            {
                AuthType = AuthType.Password,
                UserId = session.UserId,
                Lang = session.Language,
                SessionId = session.Id
            });

            jwtToken.IsSoftDelete = true;

            session.Tokens = new List<Token> { jwtToken };

            await _db.LoginAttempts.AddAsync(new LoginAttempt
            {
                Login = session.User.Login,
                Device = session.Client,
                Location = session.Location,
                IsSuccess = true,
                UserId = session.UserId,
                IsSoftDelete = true
            });

            session.Status = SessionStatus.Active;

            _db.Sessions.Update(session);

            await _db.SaveChangesAsync();

            //ToDo: Add session to storage

            return Result<JwtTokenDto>.SuccessWithData(new JwtTokenDto
            {
                ExpiredAt = jwtToken.ExpiredAt,
                RefreshToken = session.RefreshToken,
                Token = jwtToken.JwtToken
            });
        }

        public async Task<Result<JwtTokenDto>> LoginByPasswordAsync(LoginDto loginDto)
        {
            var app = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.ClientId == loginDto.App.Id && s.ClientSecret == loginDto.App.Secret);

            if (app == null)
                return Result<JwtTokenDto>.NotFound("App not found");

            if (!app.IsActive)
                return Result<JwtTokenDto>.Error("App unactive");

            if (!app.IsActiveByTime())
                return Result<JwtTokenDto>.Error("App is expired");

            var location = await _locationService.GetLocationAsync(_identityService.GetIP());

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Login == loginDto.Login);
            if (user == null)
            {
                await _db.LoginAttempts.AddAsync(new LoginAttempt
                {
                    Login = loginDto.Login,
                    Password = loginDto.Password,
                    Device = loginDto.Client,
                    Location = location,
                    IsSuccess = false,
                    IsSoftDelete = true
                });
                return Result<JwtTokenDto>.NotFound("Check your credentials");
            }

            if (!user.IsConfirmed)
                return Result<JwtTokenDto>.Error("First approve your account");


            if (user.LockoutEnabled)
            {
                if (user.IsLocked())
                {
                    return Result<JwtTokenDto>.Error($"Your account has been locked up to {user.LockoutEnd.Value.ToString("HH:mm (dd.MM.yyyy)")}");
                }

                if (user.AccessFailedCount == 5)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = DateTime.Now.AddHours(1);

                    //ToDo: send notify about blocking account

                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();

                    return Result<JwtTokenDto>.Error($"Account locked up to {user.LockoutEnd.Value.ToString("HH:mm (dd.MM.yyyy)")}");
                }
            }

            if (!loginDto.Password.VerifyPasswordHash(user.PasswordHash))
            {
                user.AccessFailedCount++;

                //ToDo: send notify about fail attempt login

                await _db.LoginAttempts.AddAsync(new LoginAttempt
                {
                    Login = loginDto.Login,
                    Password = loginDto.Password,
                    Device = loginDto.Client,
                    Location = location,
                    IsSuccess = false,
                    UserId = user.Id,
                    IsSoftDelete = true
                });
                _db.Users.Update(user);

                await _db.SaveChangesAsync();
                return Result<JwtTokenDto>.Error("Password is incorrect");
            }

            if (loginDto.Client == null)
                loginDto.Client = _detector.GetClientInfo();

            var appInfo = new AppInfo
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                ShortName = app.ShortName,
                Version = loginDto.App.Version
            };

            var session = new Session
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                App = appInfo,
                Client = loginDto.Client,
                Location = location,
                UserId = user.Id,
                RefreshToken = Guid.NewGuid().ToString("N"),
                Type = AuthScheme.Password,
                ViaMFA = user.MFA,
                IsSoftDelete = true,
                Status = SessionStatus.New,
                Language = loginDto.Lang
            };


            if (user.MFA)
            {
                await _db.Sessions.AddAsync(session);
                await _db.SaveChangesAsync();

                return Result<JwtTokenDto>.MFA(session.Id.ToString());
            }

            var jwtToken = await _tokenService.GetUserTokenAsync(new UserTokenDto
            {
                AuthType = AuthType.Password,
                UserId = user.Id,
                Lang = loginDto.Lang,
                SessionId = session.Id,
            });

            jwtToken.IsSoftDelete = true;

            session.Tokens = new List<Token>();
            session.Tokens.Add(jwtToken);

            //ToDo: create notify about success login

            await _db.LoginAttempts.AddAsync(new LoginAttempt
            {
                Login = loginDto.Login,
                Device = loginDto.Client,
                Location = location,
                IsSuccess = true,
                UserId = user.Id,
                IsSoftDelete = true
            });

            await _db.Sessions.AddAsync(session);
            await _db.SaveChangesAsync();

            //ToDo: Add session to storage

            return Result<JwtTokenDto>.SuccessWithData(new JwtTokenDto
            {
                ExpiredAt = jwtToken.ExpiredAt,
                RefreshToken = session.RefreshToken,
                Token = jwtToken.JwtToken
            });
        }

        public async Task<Result<bool>> RegisterAsync(RegisterDto registerDto)
        {
            var userWithEmail = await _db.Users.AsNoTracking().Select(s => new User { Id = s.Id, Login = s.Login, Name = s.Name }).FirstOrDefaultAsync(s => s.Login == registerDto.Login);
            if (userWithEmail != null)
                return Result<bool>.Error("Login is busy");

            if (registerDto.UserName != null && await _db.Users.AsNoTracking().AnyAsync(s => s.UserName == registerDto.UserName))
                return Result<bool>.Error("UserName is busy");

            var now = DateTime.Now;

            var newConfirm = new Confirm
            {
                ActiveFrom = now,
                ActiveTo = now.AddDays(1),
                Code = Generator.GetConfirmCode(),
                IsActivated = false,
                ActivetedAt = null
            };

            var passwordHash = registerDto.Password.GeneratePasswordHash();

            var newPassword = new Password
            {
                Answer = registerDto.KeyForPassword,
                PasswordHash = passwordHash,
                IsActive = true
            };

            var role = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(s => s.NameNormalized == DefaultsRoles.User.ToUpper());

            var newUserRole = new UserRole
            {
                Role = role,
                IsActive = true,
                ActiveFrom = now,
                ActiveTo = now.AddYears(10),
            };

            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                Login = registerDto.Login,
                PasswordHash = passwordHash,
                Email = registerDto.Login,
                IsConfirmed = false,
                IsSoftDelete = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                LockoutEnd = null,
                MFA = false,
                MFASecretKey = null,
                Name = $"{registerDto.FirstName} {registerDto.LastName}",
                Confirms = new List<Confirm> { newConfirm },
                Passwords = new List<Password> { newPassword },
                UserRoles = new List<UserRole> { newUserRole }
            };

            //ToDo: add user notification settings as json field

            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();


            //ToDo: send confirmation on email

            return Result<bool>.Success();
        }

        public async Task<Result<bool>> SendConfirmAsync(int userId)
        {
            var isUserExist = await _db.Users.AsNoTracking().AnyAsync(s => s.Id == userId);
            if (!isUserExist)
                return Result<bool>.Error("User not found");

            var now = DateTime.Now;

            var newConfirm = new Confirm
            {
                ActiveFrom = now,
                ActiveTo = now.AddDays(1),
                Code = Generator.GetConfirmCode(),
                IsActivated = false,
                ActivetedAt = null,
                UserId = userId
            };

            await _db.Confirms.AddAsync(newConfirm);
            await _db.SaveChangesAsync();

            //ToDo: send confirmation on email

            return Result<bool>.Success();
        }
    }
}
