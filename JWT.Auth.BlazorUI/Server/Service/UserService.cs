using JWT.Auth.BlazorUI.Server.data;
using JWT.Auth.BlazorUI.Shared;
using JWT.Auth.BlazorUI.Shared.Models;
using JWT.Auth.BlazorUI.Shared.registration;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JWT.Auth.BlazorUI.Shared.tokens;
using JWT.Auth.BlazorUI.Shared.login;

namespace JWT.Auth.BlazorUI.Server.Service
{
    public class UserService : IUserService
    {
        private readonly MyWorldDbContext _dbContext;
        private readonly TokenSettings _tokenSettings;
        public UserService(MyWorldDbContext dbContext,
            IOptions<TokenSettings> tokenSettings)
        {
            _dbContext = dbContext;
            _tokenSettings = tokenSettings.Value;
        }

        private User FromUserRegistrationModelToUser(UserRegistrationDto userRegistration)
        {
            return new User
            {
                Email = userRegistration.Email,
                FirstName = userRegistration.FirstName,
                Password = userRegistration.Password,
                LastName = userRegistration.LastName,
            };
        }

       

        private string HashedPassword(string plainPassword)
        {
            byte[] salt = new byte[16];
            using (var randomGenerator = RandomNumberGenerator.Create())
            {
                randomGenerator.GetBytes(salt);
            }
            var rfcPassowrd = new Rfc2898DeriveBytes(plainPassword, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassowrd.GetBytes(20);

            byte[] passwordHash = new byte[36];
            Array.Copy(salt, 0, passwordHash, 0, 16);
            Array.Copy(rfcPasswordHash, 0, passwordHash, 16, 20);

            return Convert.ToBase64String(passwordHash);
        }

        public async Task<(bool IsUserRegistered, string Message)> RegisterNewUser(UserRegistrationDto userRegistration)
        {
            var isUserExist = _dbContext.User.Any(_ => _.Email.ToLower() == userRegistration.Email.ToLower());

            if (isUserExist)
            {
                return (false, "Email Already Registered");
            }

            var newUser = FromUserRegistrationModelToUser(userRegistration);
            newUser.Password = HashedPassword(newUser.Password);

            _dbContext.User.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return (true, "Success");
        }
        private bool PasswordVerification(string plainPassword, string dbPassword)
        {
            byte[] dbPasswordHash = Convert.FromBase64String(dbPassword);

            byte[] salt = new byte[16];
            Array.Copy(dbPasswordHash, 0, salt, 0, 16);

            var rfcPassowrd = new Rfc2898DeriveBytes(plainPassword, salt, 1000, HashAlgorithmName.SHA1);
            byte[] rfcPasswordHash = rfcPassowrd.GetBytes(20);

            for (int i = 0; i < rfcPasswordHash.Length; i++)
            {
                if (dbPasswordHash[i + 16] != rfcPasswordHash[i])
                {
                    return false;
                }
            }
            return true;
        }
        private string GenerateJwtAccessToken(User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));

            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>();
            claims.Add(new Claim("Sub", user.Id.ToString()));
            claims.Add(new Claim("FirstName", user?.FirstName ?? string.Empty));
            claims.Add(new Claim("LastName", user?.LastName ?? string.Empty));
            claims.Add(new Claim("Email", user?.Email ?? string.Empty));

            var securityToken = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials,
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public async Task<(bool IsLoginSucess, JWTTokenResponseDto TokenResponse)> LoginAsync(LoginDto loginpayload)
        {
            if (string.IsNullOrEmpty(loginpayload.Email) ||
                string.IsNullOrEmpty(loginpayload.Password))
            {
                return (false, null);
            }

            var user = await _dbContext.User.Where(_ => _.Email.ToLower() == loginpayload.Email.ToLower())
                .FirstOrDefaultAsync();

            if (user == null) { return (false, null); }

            bool validUserPassowrd = PasswordVerification(loginpayload.Password, user.Password);
            if (!validUserPassowrd) { return (false, null); }

            string jwtAccessToken = GenerateJwtAccessToken(user);
            var result = new JWTTokenResponseDto
            {
                AccessToken = jwtAccessToken,
            };
            return (true, result);
        }















    }
}
