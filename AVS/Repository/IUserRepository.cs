using AVS.Models;

namespace AVS.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);
        Task<User> GetUserByID(int id);
        Task AddNewUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(int id);
    }
}
