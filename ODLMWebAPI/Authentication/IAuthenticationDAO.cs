using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Authentication
{
    public interface IAuthenticationDAO
    {
        byte[] SelectAuthenticationData(string param);
        int InsertAuthenticationData(byte[] authServerURL, byte[] clientId, byte[] clientSecret, byte[] scope);
        int ExecuteInsertionCommand(byte[] authServerURL, byte[] clientId, byte[] clientSecret, byte[] scope, SqlCommand cmdInsert);
    }
}
