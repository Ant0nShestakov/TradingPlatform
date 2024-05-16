using AVS.DB_Context;
using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AVS.Repository
{
    public class AdvertisementRepository : IRepository<Advertisement>
    {

        private readonly AppDbContext _db;

        public AdvertisementRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Advertisement model)
        {
            _db.Advertisements.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var advertisement = await _db.Advertisements.FindAsync(id);
            if (advertisement == null)
                return;
            _db.Advertisements.Remove(advertisement);
            await _db.SaveChangesAsync();
        }

        public async Task<Advertisement?> GetById(Guid? id)
        {
            var advertisement = await _db.Advertisements
                .Include(adv => adv.Photos).Include(adv => adv.AdvertisementState)
                .Include(adv => adv.Category)
                .Include(adv => adv.Address).FirstOrDefaultAsync(adv => adv.ID == id);

            if (advertisement == null)
                return null;
            
            return advertisement;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisements() => await _db.Advertisements.ToListAsync();

        public async Task<List<Advertisement>>? GetAllAdvertisementByUserId(Guid userId)
        {
            List<Advertisement> advertisements = await _db.Advertisements
                .Include(adv => adv.Photos).Include(adv => adv.AdvertisementState)
                .Include(adv => adv.Category)
                .Include(adv => adv.Address).ToListAsync();

            if (advertisements == null)
                return null;

            return advertisements;
        }

        public async Task Update(Advertisement model)
        {
            _db.Advertisements.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
