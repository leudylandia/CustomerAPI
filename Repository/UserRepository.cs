using CustomerAPI.Data;
using CustomerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<string> Login(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username));

            if (user == null)
                return "nouser";

            if (!verificarPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return "passwordwrong";

            return "ok";

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
    }
}
