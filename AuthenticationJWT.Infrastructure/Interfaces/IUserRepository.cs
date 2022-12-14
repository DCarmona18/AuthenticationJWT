using AuthenticationJWT.Infrastructure.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAndPassword(string userName, string password);
    }
}
