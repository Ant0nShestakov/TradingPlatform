using AVS.DB_Context;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db = null!;

        public UserRepository(AppDbContext db) 
        {
            this._db = db;
        }

        public async Task AddNewUser(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email) => 
            await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> GetUserByID(int id) => await _db.Users.FindAsync(id);

        public async Task UpdateUser(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
