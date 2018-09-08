using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _ctx;
        public AuthRepository(DataContext ctx)
        {
            this._ctx = ctx;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            if (!VerifyPassword(password, user.PasswordHash, user.Salt)) return null;

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return passwordHash.SequenceEqual(computedHash);
            }
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] passwordHash, salt;

            CreatePasswordHash(password, out passwordHash, out salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;

            await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _ctx.Users.AnyAsync(u => u.Username == username);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}