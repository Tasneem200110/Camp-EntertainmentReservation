using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetById(int UserId);
        public Task<User> GetUserByEmail(string Email);
        Task AddUser(User user);
        Task Update(User user);
        Task DeleteUser(int UserId);
    }
}
