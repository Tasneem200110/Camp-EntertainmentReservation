using DAL.Entities;

namespace BLL.Interfaces
{

    public interface IUserRepository
    {
        public Task<User> GetById(int UserId);
        public Task<User> GetUserByEmail(string Email);
        public Task<IEnumerable<User>> GetUsers();
        Task AddUser(User user);
        Task Update(User user);
        Task DeleteUser(int UserId);
        Task<int> GetUsersCount();
        Task<int> GetTodayNewUsers();
        public int? GetUserId();
    }
}