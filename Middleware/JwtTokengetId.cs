using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerse_shoes_backend.Middleware
{
    public class JwtTokengetId:IJwtTokengetId
    {
        private readonly string skey;
        private readonly IConfiguration _configuration;

        public JwtTokengetId(IConfiguration configuration)
        {
            _configuration = configuration;
            skey = _configuration["Jwt:Key"];
        }

        public int GetUserId(string token)
        {
            var tokenhandler=new JwtSecurityTokenHandler();
            var securitykey=Encoding.UTF8.GetBytes(skey);
            var validationparameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securitykey),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            var validation = tokenhandler.ValidateToken(token, validationparameter, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
            {
                throw new SecurityTokenException("invalid token");
            }

            var userid = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (userid==null || !int.TryParse(userid.Value,out var UserId))
            {
                throw new SecurityTokenException("invalid token");

            }
            return UserId;
        }
    }
}
