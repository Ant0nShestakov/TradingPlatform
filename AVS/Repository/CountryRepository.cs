using AVS.DB_Context;
using AVS.Models.AddressModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class CountryRepository : IRepository<Country>
    {

        private readonly AppDbContext _db;

        public CountryRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Country model)
        {
            _db.Countries.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var country = await _db.Countries.FindAsync(id);
            if (country == null)
                return;
            _db.Countries.Remove(country);
            await _db.SaveChangesAsync();
        }

        public async Task<Country?> GetById(Guid id)
        {
            var country = await _db.Countries.FirstOrDefaultAsync(country => country.Id == id);

            if (country == null)
                return null;
            
            return country;
        }

        public async Task<IEnumerable<Country>> GetAllCountry() => await _db.Countries.ToListAsync();

        public async Task Update(Country model)
        {
            _db.Countries.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
