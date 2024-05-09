using AVS.DB_Context;
using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class RegionsRepository : IRepository<Region>
    {

        private readonly AppDbContext _db;

        public RegionsRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Region model)
        {
            _db.Regions.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var region = await _db.Regions.FindAsync(id);
            if (region == null)
                return;
            _db.Regions.Remove(region);
            await _db.SaveChangesAsync();
        }

        public async Task<Region?> GetById(Guid id)
        {
            var region = await _db.Regions.FirstOrDefaultAsync(r => r.ID == id);

            if (region == null)
                return null;
            
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllRegions() => await _db.Regions.ToListAsync();

        public async Task<IEnumerable<Region>> GetAllRegionsByCountryId(Guid countryId) =>
            await _db.Regions.Where(region => region.CountryID == countryId).ToListAsync();

        public async Task Update(Region model)
        {
            _db.Regions.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
