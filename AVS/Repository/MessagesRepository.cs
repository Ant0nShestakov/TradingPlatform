using AVS.DB_Context;
using AVS.Models.UserModels;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Message>> GetMessagesBetweenUsers(Guid userId1, Guid userId2)
        {
            return await _appDb.Messages
                .Where(m => ((m.SenderUserId == userId1 && m.ReceiverUserId == userId2) ||
                            (m.SenderUserId == userId2 && m.ReceiverUserId == userId1)))
                .Include(m => m.SenderUser)
                .Include(m => m.ReceiverUser)
                .ToListAsync();
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
