using MainEnvironment.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MainEnvironment.Web.Services
{
    public class SecureTokenService : ISecureTokenService
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int MAX_SIZE = 50;
        public string GenerateSecureToken(int size)
        {
            string token = null;
            if (size > 0 && size < MAX_SIZE)
            {
                byte[] data = new byte[4 * size];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    var rnd = BitConverter.ToUInt32(data, i * 4);
                    int idx = (int)(rnd % ALPHABET.Length);

                    result.Append(ALPHABET[idx]);
                }
                token = result.ToString();
            }

            return token;
        }
    }
}
