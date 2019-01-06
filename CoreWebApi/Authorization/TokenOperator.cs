using CoreWebApi.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreWebApi.Authorization
{
    public class TokenOperator
    {
        public static string GenerateToken(UserInfoDto user)
        {
            var jti = user.UserName + DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var claims = new Claim[]
            {
                // new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.Role, user.Type),
                new Claim(JwtRegisteredClaimNames.Iss, "Core Web Api"),
                // new Claim(JwtRegisteredClaimNames.Aud, "Web Application"),
                // new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                // new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim("company", user.Company),
                new Claim("department", user.Department),
                new Claim("position", user.Position)
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            double uts = Convert.ToDouble(unixTimeStamp);
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(uts).ToLocalTime();
            return dtDateTime;
        }
    }
}
