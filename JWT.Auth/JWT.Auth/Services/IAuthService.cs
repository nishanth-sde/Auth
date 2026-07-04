using JWT.Auth.Configurations;
using JWT.Auth.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT.Auth.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        string HashPassword(string password);
    }
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly AuthConfig _authConfig;
        private const int hashSize = 32;
        private const int saltSize = 16;
        private const int iterdations = 100000;
        private static readonly HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _authConfig = configuration.GetSection("Jwt").Get<AuthConfig>() ?? throw new Exception("Jwt configuration is missing");
        }

        public async Task<string?> LoginAsync(
            string email,
            string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User not exists");
            }
            bool isvalidUser = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isvalidUser)
            {
                throw new Exception("Invalid password");
            }
            return GenerateJwtToken(user.Id, user.Email, user.Username);
        }
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterdations,
                algorithm,
                hashSize);
            string hashedPassword = $"{Convert.ToHexString(salt)}-{Convert.ToHexString(hash)}";
            string[] parts = hashedPassword.Split("-");
            string hashed = parts[1];
            string defaultPassword = "Nishanth";
            byte[] defaulthash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(defaultPassword),
                salt,
                iterdations,
                algorithm,
                hashSize);
            bool isSame = hash.SequenceEqual(defaulthash);
            Console.WriteLine(isSame);

            return $"{Convert.ToHexString(salt)}-{Convert.ToHexString(hash)}";


        }
        private string GenerateJwtToken(
            int userId,
            string email,
            string username)
        {
            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier,userId.ToString()),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Name,username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: _authConfig.Issuer, audience: _authConfig.Audience, expires: DateTime.Now.AddMinutes(_authConfig.ExpirationInMin), claims: claims, signingCredentials: creds);
            return new JwtSecurityTokenHandler()
                .WriteToken(token);

        }
        
    }
}
        


