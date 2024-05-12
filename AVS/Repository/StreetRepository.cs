using AVS.DB_Context;
using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class StreetRepository : IRepository<Street>
    {

        private readonly AppDbContext _db;

        public StreetRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Street model)
        {
            _db.Streets.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var street = await _db.Streets.FindAsync(id);
            if (street == null)
                return;
            _db.Streets.Remove(street);
            await _db.SaveChangesAsync();
        }

        public async Task<Street?> GetById(Guid id)
        {
            var street = await _db.Streets
                .Include(s => s.Locality).ThenInclude(l => l.Region)
                .ThenInclude(r => r.Country).FirstOrDefaultAsync(s => s.Id == id);

            if (street == null)
                return null;
            
            return street;
        }

        public async Task<IEnumerable<Street>> GetAllStreet() => await _db.Streets.ToListAsync();

        public async Task<IEnumerable<Street>> GetAllStreetByLocalityId(Guid localityID) =>
            await _db.Streets.Where(street => street.LocalityID == localityID).ToListAsync();

        public async Task Update(Street model)
        {
            _db.Streets.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
