using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.Authentication
{   
    // Vaibhav [20-Mar-2018] Added to encrypt auth server parameters.
    public class Authentications : IAuthentication
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IAuthenticationDAO _iAuthenticationDAO;
        public Authentications(ITblConfigParamsDAO iTblConfigParamsDAO, IAuthenticationDAO iAuthenticationDAO)
        {

            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iAuthenticationDAO = iAuthenticationDAO;
        }

        public static string EncryptDescryptpassword = "yHparGoTPyrc";
        public static byte[] EncryptDescryptKey = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        #region Cryptography

        public  string DecryptData(byte[] paramByte)
        {
            try
            {
                using (Aes decryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptDescryptpassword, EncryptDescryptKey);
                    decryptor.Key = pdb.GetBytes(32);
                    decryptor.IV = pdb.GetBytes(16);

                    using (var decryptedStream = new MemoryStream())
                    {
                        using (ICryptoTransform iDecryptor = decryptor.CreateDecryptor())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(decryptedStream, iDecryptor, CryptoStreamMode.Write))
                            {
                                using (var originalByteStream = new MemoryStream(paramByte))
                                {
                                    int data;
                                    while ((data = originalByteStream.ReadByte()) != -1)
                                        cryptoStream.WriteByte((byte)data);
                                }

                                string d = Encoding.Unicode.GetString(decryptedStream.ToArray());
                            }
                        }
                        return Encoding.Unicode.GetString(decryptedStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public  byte[] EncryptData(string param)
        {
            byte[] fileStreamBytes = Encoding.Unicode.GetBytes(param);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptDescryptpassword, EncryptDescryptKey);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var encryptedStream = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(encryptedStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var fileEncryptedStream = new MemoryStream(fileStreamBytes))
                        {
                            int data;
                            while ((data = fileEncryptedStream.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }

                    string res = Encoding.Unicode.GetString(encryptedStream.ToArray());
                    return encryptedStream.ToArray();

                }
            }
        }

        #endregion


        public AuthenticationTO getAccessToken(string userName, string password)
        {
            Uri authorizationServerTokenIssuerUri = new Uri("http://52.172.182.199/AuthenticationServer/connect/token");
            string clientId = "ODLMK";
            string clientSecret = "secret";
            string scope = "api1";

            TblConfigParamsTO authenticationURLTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_AUTHENTICATION_URL);
            TblConfigParamsTO clientIdTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_CLIENT_ID);
            TblConfigParamsTO clientSecretTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_CLIENT_SECRET);
            TblConfigParamsTO scopeTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SCOPE);

            //Uri authorizationServerTokenIssuerUri = new Uri(DecryptData(AuthenticationDAO.SelectAuthenticationData(authenticationURLTO.ConfigParamVal)));
            //string clientId = DecryptData(AuthenticationDAO.SelectAuthenticationData(clientIdTO.ConfigParamVal));
            //string clientSecret = DecryptData(AuthenticationDAO.SelectAuthenticationData(clientSecretTO.ConfigParamVal));
            //string scope = DecryptData(AuthenticationDAO.SelectAuthenticationData(scopeTO.ConfigParamVal));

            //access token request
            string rawJwtToken = RequestTokenToAuthorizationServer(
                 authorizationServerTokenIssuerUri,
                 clientId,
                 scope,
                 clientSecret, userName, password)
                .GetAwaiter()
                .GetResult();

            AuthenticationTO authorizationServerToken;
            return authorizationServerToken = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthenticationTO>(rawJwtToken);

        }

        private async Task<string> RequestTokenToAuthorizationServer(Uri uriAuthorizationServer, string clientId, string scope, string clientSecret, string userName, string password)
        {
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, uriAuthorizationServer);
                HttpContent httpContent = new FormUrlEncodedContent(
                    new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("scope", scope),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("username",userName),
                    new KeyValuePair<string, string>("password",password)
                    });
                tokenRequest.Content = httpContent;
                responseMessage = await client.SendAsync(tokenRequest);
            }
            return await responseMessage.Content.ReadAsStringAsync();
        }

        public void InsertAuthenticationData()
        {
            byte[] authServerURL = EncryptData("http://52.172.182.199/AuthenticationServer/connect/token");
            byte[] clientId = EncryptData("ODLMK");
            byte[] clientSecret = EncryptData("secret");
            byte[] scope = EncryptData("api1");

            _iAuthenticationDAO.InsertAuthenticationData(authServerURL,clientId,clientSecret,scope);
        }
    }
}
