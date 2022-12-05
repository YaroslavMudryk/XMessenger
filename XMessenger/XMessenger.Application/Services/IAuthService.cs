using Extensions.DeviceDetector;
using Extensions.Password;
using Microsoft.EntityFrameworkCore;
using XMessenger.Application.Dtos;
using XMessenger.Application.Dtos.Identity;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers.Identity;
using XMessenger.Helpers.Services;
using XMessenger.Infrastructure.Data.EntityFramework.Context;

namespace XMessenger.Application.Services
{
    public interface IAuthService
    {
        Task<Result<JwtTokenDto>> LoginByPassword(LoginDto loginDto);
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

        public async Task<Result<JwtTokenDto>> LoginByPassword(LoginDto loginDto)
        {
            var app = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(s => s.ClientId == loginDto.App.Id && s.ClientSecret == loginDto.App.Secret);

            if (app == null)
                return Result<JwtTokenDto>.NotFound("App not found");

            if (!app.IsActive)
                return Result<JwtTokenDto>.Error("App unactive");

            if (!app.IsActiveByTime())
                return Result<JwtTokenDto>.Error("App is expired");

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Login == loginDto.Login);
            if (user == null)
                return Result<JwtTokenDto>.NotFound("Check your credentials");


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

                _db.Users.Update(user);

                await _db.SaveChangesAsync();
                return Result<JwtTokenDto>.Error("Password is incorrect");
            }

            if (loginDto.Client == null)
                loginDto.Client = _detector.GetClientInfo();

            var location = await _locationService.GetLocationAsync(_identityService.GetIP());

            var sessionId = Guid.NewGuid();

            var session = new Session
            {
                Id = sessionId,
                IsActive = true,
                App = null,
                Client = loginDto.Client,
                Location = location,
                UserId = user.Id,
                RefreshToken = Guid.NewGuid().ToString("N"),
                Type = AuthScheme.Password,
                ViaMFA = user.MFA
            };

            var jwtToken = await _tokenService.GetUserTokenAsync(new UserTokenDto
            {
                AuthType = AuthType.Password,
                UserId = user.Id,
                Lang = loginDto.Lang,
                SessionId = sessionId,
            });

            session.Tokens = new List<Token>();
            session.Tokens.Add(jwtToken);

            //ToDo: create notify about success login


            await _db.Sessions.AddAsync(session);
            await _db.SaveChangesAsync();

            if (user.MFA)
                return Result<JwtTokenDto>.MFA(sessionId.ToString());

            return Result<JwtTokenDto>.SuccessWithData(new JwtTokenDto
            {
                ExpiredAt = jwtToken.ExpiredAt,
                RefreshToken = session.RefreshToken,
                Token = jwtToken.JwtToken
            });
        }
    }
}
