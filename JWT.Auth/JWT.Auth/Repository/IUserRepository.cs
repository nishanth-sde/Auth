using JWT.Auth.Data;
using JWT.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT.Auth.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
