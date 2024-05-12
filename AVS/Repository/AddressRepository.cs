using AVS.DB_Context;
using AVS.Models.AddressModels;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AVS.Repository
{
    public class AddressRepository : IRepository<Address>
    {

        private readonly AppDbContext _db;

        public AddressRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Address model)
        {
            _db.Addressies.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var address = await _db.Addressies.FindAsync(id);
            if (address == null)
                return;
            _db.Addressies.Remove(address);
            await _db.SaveChangesAsync();
        }

        public async Task<Address?> GetById(Guid id)
        {
            var address = await _db.Addressies.FirstOrDefaultAsync(a => a.ID == id);

            if (address == null)
                return null;
            
            return address;
        }


        public async Task Update(Address model)
        {
            _db.Addressies.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
