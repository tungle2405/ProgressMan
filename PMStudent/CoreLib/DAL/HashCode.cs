using CoreLib.BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.DAL
{
    public class HashCode : IHashCode
    {
        public string Encrypt(string key)
        {
            string hash = "Pa$$w0rd";
            byte[] data = UTF8Encoding.UTF8.GetBytes(key);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string key)
        {
            string hash = "Pa$$w0rd";
            byte[] data = Convert.FromBase64String(key);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider();

            tripDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripDES.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripDES.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
