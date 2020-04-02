using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace NorthwindRestApi.Tools
{
    // https://dotnetcoretutorials.com/2020/01/15/creating-and-validating-jwt-tokens-in-asp-net-core/
    public class TokenGenerator
    {
        public static string GenerateToken(string username)
        {
            var munSecret = "123456789abcdefghijklmnopqrstuvxyz";
            var munSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(munSecret));

            var munIssuer = "http://petrila.azurewebsites.net/";
            var munAudience = "http://www.sharewell.fi/react2020/oppilas16/";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = munIssuer,
                Audience = munAudience,
                SigningCredentials = new SigningCredentials(munSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); 


        }


    }
}
