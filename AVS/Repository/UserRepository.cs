using AVS.DB_Context;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _db = null!;

        public UserRepository(AppDbContext db) 
        {
            this._db = db;
        }

        #region GETs
        public async Task<User?> GetUserByEmail(string email) => await _db.Users.Include(u => u.Roles)
            .Include(u => u.Advertisements)
            .Include(u => u.Messages)
            .FirstOrDefaultAsync(user => user.Email == email);

        public async Task<User?> GetById(Guid id) => await _db.Users.FirstOrDefaultAsync(user => user.Id == id);

        public async Task<User?> GetUserByAdvertisementId(Guid id) => 
            await _db.Users.FirstOrDefaultAsync(user => user.Advertisements.Any(advertisement => advertisement.ID == id));

        //public async Task<User?> GetUserByMessageId(Guid id) => 
        //    await _db.Users.FirstOrDefaultAsync(user => user.Messages.Any(message => message.Id == id));

        public async Task<List<User>?> GetUsersByRoleId(Guid id) => await _db.Users.Where(user => user.Roles.Any(role => role.Id == id)).ToListAsync();
        #endregion

        #region INSERTs
        public async Task Add(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        //public async Task AddRoleToUser(Guid id, Role role)
        //{
        //    var user = await _db.Users.Include(user => user.Roles).FirstOrDefaultAsync(user => user.Id == id);

        //    if (user == null)
        //        return;

        //    if(!user.Roles.Any(role => role.Id == role.Id))
        //    {
        //        user.Roles.Add(role);
        //        await _db.SaveChangesAsync();
        //    }
        //    return;
        //}
        #endregion

        public async Task DeleteById(Guid id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return;
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
