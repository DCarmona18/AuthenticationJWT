using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<string> BuildToken(string key, string issuer, IEnumerable<string> audience, string userName, string password);
    }
}
