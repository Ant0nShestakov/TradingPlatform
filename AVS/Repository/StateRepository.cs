using AVS.DB_Context;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class StateRepository : IRepository<AdvertisementState>
    {

        private readonly AppDbContext _db;

        public StateRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(AdvertisementState model)
        {
            _db.AdvertisementStates.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var advertisementState = await _db.AdvertisementStates.FindAsync(id);
            if (advertisementState == null)
                return;
            _db.AdvertisementStates.Remove(advertisementState);
            await _db.SaveChangesAsync();
        }

        public async Task<AdvertisementState?> GetById(Guid id)
        {
            var advertisementState = await _db.AdvertisementStates.FirstOrDefaultAsync(s => s.ID == id);

            if (advertisementState == null)
                return null;
            
            return advertisementState;
        }

        public async Task<IEnumerable<AdvertisementState>> GetAllState() => await _db.AdvertisementStates.ToListAsync();

        public async Task Update(AdvertisementState model)
        {
            _db.AdvertisementStates.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
