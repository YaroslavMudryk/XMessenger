using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using XMessenger.Identity.Models;
using XMessenger.Helpers;
using XMessenger.Helpers.Identity;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Dtos;

namespace XMessenger.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly IdentityContext _db;
        public TokenService(IdentityContext db)
        {
            _db = db;
        }


        public async Task<Token> GetUserTokenAsync(UserTokenDto userToken)
        {
            userToken.Lang = userToken.Lang.ToLower();

            var user = userToken.User ?? await _db.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userToken.UserId);

            var session = userToken.Session ?? await _db.Sessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == userToken.SessionId);

            var userRoles = await _db.UserRoles.AsNoTracking().Include(s => s.Role).Where(s => s.UserId == userToken.UserId).Select(s => s.Role).ToListAsync();

            var userRolesIds = userRoles.Select(s => s.Id);

            var claims = new List<System.Security.Claims.Claim>();

            claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.UserId, user.Id.ToString()));
            claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.Login, user.Login));
            claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.SessionId, session.Id.ToString()));
            claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.AuthenticationMethod, session.Type));
            claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.Language, session.Language));

            foreach (var role in userRoles)
            {
                claims.Add(new System.Security.Claims.Claim(ConstantsClaimTypes.Role, role.Name));
            }

            var claimsFromRole = await _db.RoleClaims.AsNoTracking().Include(s => s.Claim).Where(s => userRolesIds.Contains(s.RoleId)).Select(s => s.Claim).ToListAsync();

            var claimsFromApp = await _db.AppClaims.AsNoTracking().Include(s => s.Claim).Where(s => s.AppId == session.App.Id).Select(s => s.Claim).ToListAsync();

            var claimsForToken = GetUniqClaims(new List<IEnumerable<XMessenger.Identity.Models.Claim>> { claimsFromRole, claimsFromApp });

            foreach (var claim in claimsForToken)
            {
                claims.Add(new System.Security.Claims.Claim(claim.Type, claim.Value));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);


            var now = DateTime.Now;
            var expiredAt = now.AddMinutes(TokenOptions.LifeTimeInMinutes);
            var jwt = new JwtSecurityToken(
                issuer: TokenOptions.Issuer,
                audience: TokenOptions.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: expiredAt,
                signingCredentials: new SigningCredentials(TokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new Token
            {
                JwtToken = encodedJwt,
                ExpiredAt = expiredAt,
                SessionId = session.Id,
                Session = session
            };
        }

        private List<XMessenger.Identity.Models.Claim> GetUniqClaims(IEnumerable<IEnumerable<XMessenger.Identity.Models.Claim>> source)
        {
            var claims = new List<XMessenger.Identity.Models.Claim>();

            foreach (var enumerable in source)
            {
                foreach (var claim in enumerable)
                {
                    if (!claims.Any(s => s.Type == claim.Type && s.Value == claim.Value))
                        claims.Add(claim);
                }
            }

            return claims;
        }
    }
}
