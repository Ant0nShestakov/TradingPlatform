using AVS.DB_Context;
using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class LocalitiesRepository : IRepository<Locality>
    {

        private readonly AppDbContext _db;

        public LocalitiesRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Locality model)
        {
            _db.Localities.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var locality = await _db.Localities.FindAsync(id);
            if (locality == null)
                return;
            _db.Localities.Remove(locality);
            await _db.SaveChangesAsync();
        }

        public async Task<Locality?> GetById(Guid? id)
        {
            var locality = await _db.Localities.FirstOrDefaultAsync(l => l.ID == id);

            if (locality == null)
                return null;
            
            return locality;
        }

        public async Task<IEnumerable<Locality>> GetAllLocalities() => await _db.Localities
            .OrderBy(locality => locality.Name).ToListAsync();

        public async Task<IEnumerable<Locality>> GetLocalitieByRegionId(Guid regionId) =>
            await _db.Localities.Where(locality => locality.RegionID == regionId).ToListAsync();

        public async Task Update(Locality model)
        {
            _db.Localities.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
