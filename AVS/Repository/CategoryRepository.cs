using AVS.DB_Context;
using AVS.Models.AdvertisementModels;
using Microsoft.EntityFrameworkCore;

namespace AVS.Repository
{
    public class CategoryRepository : IRepository<Category>
    {

        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext context) 
        {
            this._db = context;
        }

        public async Task Add(Category model)
        {
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var category = await _db.Countries.FindAsync(id);
            if (category == null)
                return;
            _db.Countries.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task<Category?> GetById(Guid? id)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(category => category.Id == id);

            if (category == null)
                return null;
            
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategories() => await _db.Categories
            .OrderBy(category => category.Name).ToListAsync();

        public async Task Update(Category model)
        {
            _db.Categories.Update(model);
            await _db.SaveChangesAsync();
        }
    }
}
