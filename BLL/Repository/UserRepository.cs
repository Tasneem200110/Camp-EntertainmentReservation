using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MvcAppDbContext _context;

        public UserRepository(MvcAppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetById(int UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
        }

        public async Task<User> GetUserByEmail(string Email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Update(User user)
        {
            var exist = await _context.Users.FindAsync(user.Id);
            if (exist != null)
            {
                exist.UserName = user.UserName;
                exist.Email = user.Email;
                exist.PasswordHash = user.PasswordHash;

                _context.Users.Update(exist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUsersCount()
        {
            return await _context.Users.CountAsync();
        }
    }
}