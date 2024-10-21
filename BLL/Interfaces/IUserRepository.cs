using DAL.Entities;

namespace BLL.Interfaces;

public interface IUserRepository
{
    public Task<User> GetById(int UserId);
    public Task<User> GetUserByEmail(string Email);
    Task AddUser(User user);
    Task Update(User user);
    Task DeleteUser(int UserId);
}