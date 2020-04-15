using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SlotGame.API.Options;
using SlotGame.Types.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SlotGame.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtSettings jwtSettings;
        public IdentityService(UserManager<IdentityUser> _userManager, JwtSettings _jwtSettings)
        {
            userManager = _userManager;
            jwtSettings = _jwtSettings;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return new AuthenticationResult { Errors = new[] { "User with email address does not exist" } };

            var userHasValidPassword = await userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
                return new AuthenticationResult { Errors = new[] { "Username or Password is wrong" } };

            return GenerateAuthenticationResultForUser(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
                return new AuthenticationResult { Errors = new[] { "User with email address does exist" } };

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createdUser = await userManager.CreateAsync(newUser, password);

            return GenerateAuthenticationResultForUser(user);         
        }

        private AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser newUser)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
            };
        }
    }
}
