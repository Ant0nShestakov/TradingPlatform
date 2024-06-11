using AVS.DB_Context;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;

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
                .Include(adv => adv.Category).Include(adv => adv.User)
                .Include(adv => adv.Address).FirstOrDefaultAsync(adv => adv.ID == id);



            if (advertisement == null)
                return null;

            advertisement.Address.Street = await _db.Streets.Include(street => street.Locality)
                .FirstOrDefaultAsync(street => street.Id == advertisement.Address.StreetID);

            advertisement.Address.Street.Locality.Region = await _db.Regions
                .Include(region => region.Country)
                .FirstOrDefaultAsync(region => region.ID == advertisement.Address.Street.Locality.RegionID);

            return advertisement;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisements()
        {
            var adveristements = await _db.Advertisements
            .Include(advertisement => advertisement.Photos)
            .Include(advertisement => advertisement.Address)
            .OrderByDescending(adveristement => adveristement.NumberOfViews)
            .ToListAsync();

            foreach(var advertisement in adveristements)
            {
                advertisement.Address.Street = await _db.Streets.Include(street => street.Locality)
                    .FirstOrDefaultAsync(street => street.Id == advertisement.Address.StreetID);

                advertisement.Address.Street.Locality.Region = await _db.Regions.Include(region => region.Country)
                    .FirstOrDefaultAsync(region => region.ID == advertisement.Address.Street.Locality.RegionID);
            }
            return adveristements;
        }

        public async Task<List<Advertisement>> GetByIds(List<Guid> ids)
        {
            return await _db.Advertisements.Where(a => ids.Contains(a.ID))
                .Include(adv => adv.Photos)
                .ToListAsync();
        }

        public async Task<List<Advertisement>>? GetAllAdvertisementByUserId(Guid userId)
        {
            List<Advertisement> advertisements = await _db.Advertisements
                .Where(advertisement => advertisement.UserId == userId)
                .Include(adv => adv.Photos).Include(adv => adv.AdvertisementState)
                .Include(adv => adv.Category)
                .Include(adv => adv.Address).ToListAsync();

            if (advertisements == null)
                return null;

            return advertisements;
        }

        public async Task<List<Advertisement>> GetAllAdvertisementByCategoryId(Guid categoryId)
        {
            List<Advertisement> advertisements = await _db.Advertisements
                .Include(adv => adv.Address)
                .Include(adv => adv.Photos)
                .Include(adv => adv.Category).Where(adv => adv.CategoryId == categoryId).ToListAsync();

            foreach(var advertisement in advertisements)
            {
                advertisement.Address.Street = await _db.Streets.Include(street => street.Locality)
                    .FirstOrDefaultAsync(street => street.Id == advertisement.Address.StreetID);

                advertisement.Address.Street.Locality.Region = await _db.Regions.Include(region => region.Country)
                    .FirstOrDefaultAsync(region => region.ID == advertisement.Address.Street.Locality.RegionID);
            }
            return advertisements;
        }

        public async Task<IEnumerable<Advertisement>> GetAllAdvertisementByLocalityId(Guid LocalityId)
        {
            IEnumerable<Advertisement> advertisements = await _db.Advertisements
                .Include(adv => adv.Address)
                .Include(adv => adv.Category)
                .Include(adv => adv.Photos)
                .ToListAsync();

            List<Advertisement> filtredAdvertisement = new();

            foreach (var advertisement in advertisements)
            {
                advertisement.Address.Street = await _db.Streets.Include(street => street.Locality)
                    .FirstOrDefaultAsync(street => street.Id == advertisement.Address.StreetID);

                if (advertisement.Address.Street.LocalityID != LocalityId)
                    continue;

                advertisement.Address.Street.Locality.Region = await _db.Regions.Include(region => region.Country)
                    .FirstOrDefaultAsync(region => region.ID == advertisement.Address.Street.Locality.RegionID);

                filtredAdvertisement.Add(advertisement);
            }

            return filtredAdvertisement;
        }

        public async Task Update(Advertisement model)
        {
            _db.Advertisements.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
