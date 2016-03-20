

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace cloudscribe.SimpleContent.Services
{
    public class CryptoHelper
    {
        public CryptoHelper()
        {
            encoder = new UTF8Encoding();
            md5Hasher = MD5.Create();
        }

        private MD5 md5Hasher;
        private UTF8Encoding encoder;

        public string GenerateMd5Hash(string inputString)
        {
            if(string.IsNullOrEmpty(inputString)) { return inputString; }

            byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(inputString));

            StringBuilder sb = new StringBuilder(hashedBytes.Length * 2);
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("X2"));
            }


            return sb.ToString().ToLower();
        }

    }
}
