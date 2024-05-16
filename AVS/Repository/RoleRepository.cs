using AVS.DB_Context;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class RoleRepository : IRepository<Role>
    {
        private readonly AppDbContext _db = null!;

        public RoleRepository(AppDbContext db)
        {
            this._db = db;
        }

        public async Task<Role?> GetById(Guid? id) => await _db.Roles.FirstOrDefaultAsync(role => role.Id == id);
        public async Task<Role?> GetByName(string name) => await _db.Roles.FirstOrDefaultAsync(role => role.Name.Equals(name));

        public async Task Add(Role model)
        {
            _db.Roles.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var role = await _db.Roles.FindAsync(id);
            if (role == null)
                return;
            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();
        }

        public async Task Update(Role role)
        {
            _db.Roles.Update(role);
            await _db.SaveChangesAsync();
        }
    }
}
