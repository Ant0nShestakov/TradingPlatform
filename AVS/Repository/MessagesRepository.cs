using AVS.DB_Context;
using AVS.Models.AdvertisementModels;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AVS.Repository
{
    public class MessagesRepository : IRepository<Message>
    {
        private readonly AppDbContext _appDb;

        public MessagesRepository(AppDbContext appDb)
        {
            _appDb = appDb;
        }

        public async Task Add(Message model)
        {
            if (model == null)
                throw new NullReferenceException(nameof(model));

            _appDb.Messages.Add(model);
            await _appDb.SaveChangesAsync();
        }

        public Task DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Message?> GetById(Guid? id)
        {
            return await _appDb.Messages.FirstOrDefaultAsync(message => message.Id == id);
        }

        public async Task<List<Message>> GetAllMessageBySenderAndReciever(User senderId, User receiverId)
        {
            return null;
           
        }


        public async Task Update(Message model)
        {
            if (model == null)
                throw new NullReferenceException(nameof(model));

            _appDb.Messages.Update(model);
            await _appDb.SaveChangesAsync();
        }
    }
}
