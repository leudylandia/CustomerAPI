using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
        }
        public async Task<string> Login(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username));

            if (user == null)
                return "nouser";

            if (!verificarPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return "passwordwrong";

            return CrearToken(user);

        }

        public async Task<int> Resgister(User user, string password)
        {
            try
            {
                if (await UserExist(user.UserName))
                    return -1; //usuario Existe

                CrearPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception)
            {
                return -500;
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName.ToLower().Equals(username.ToLower())))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Crear password
        /// </summary>
        /// <param name="password">Contraseña</param>
        /// <param name="passwordHasd">passwordSalt</param>
        /// <param name="passwordSalt">passwordHasd</param>
        private void CrearPasswordHash(string password, out byte[] passwordHasd, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHasd = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool verificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string CrearToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var tokenInfo = _configuration["AppSettings:Token"];
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
