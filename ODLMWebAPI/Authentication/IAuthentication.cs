using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Authentication
{
    public interface IAuthentication
    {
        AuthenticationTO getAccessToken(string userName, string password);
        void InsertAuthenticationData();
    }
}
