using AuthenticationJWT.Infrastructure.Context;
using AuthenticationJWT.Infrastructure.Context.Entities;
using AuthenticationJWT.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAndPassword(string userName, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == userName && x.Password == password);
        }
    }
}
