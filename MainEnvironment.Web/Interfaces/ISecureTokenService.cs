using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Interfaces
{
    public interface ISecureTokenService
    {
        string GenerateSecureToken(int size);
    }
}
