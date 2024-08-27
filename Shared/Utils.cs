using auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.Shared
{
    public class Utils(
        IConfiguration configuration,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<User> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSetting").GetSection("SecurityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = [
                new (JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new (JwtRegisteredClaimNames.Name, user.FullName ?? ""),
                new (JwtRegisteredClaimNames.NameId, user.Id ?? ""),
                new (JwtRegisteredClaimNames.Aud, _configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value?? ""),
                new (JwtRegisteredClaimNames.Iss, _configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value?? ""),
            ];

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
