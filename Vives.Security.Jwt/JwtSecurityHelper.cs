using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security;

namespace Vives.Security.Jwt
{
    public static class JwtSecurityHelper
    {
        public static ClaimsPrincipal GetClaimsPrincipal(string token, string scheme)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken is null)
            {
                throw new SecurityException("Token could not be converted");
            }

            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.ToLower() == "name");
            var claims = jwtToken.Claims.ToList();

            if (nameClaim is not null)
            {
                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
            }

            var claimsIdentity = new ClaimsIdentity(claims, scheme);

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
