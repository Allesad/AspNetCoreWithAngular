using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DatingApp.Api.Models;
using Newtonsoft.Json;

namespace DatingApp.Api.Data
{
    public class Seed
    {
        private readonly DataContext _ctx;

        public Seed(DataContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedUsers()
        {
            var userData = File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            byte[] passwordHash, salt;
            CreatePasswordHash("password", out passwordHash, out salt);
            
            _ctx.Users.AddRange(users.Select(u => {
                u.PasswordHash = passwordHash;
                u.Salt = salt;
                u.Username = u.Username.ToLower();
                return u;
            }));
            _ctx.SaveChanges();
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