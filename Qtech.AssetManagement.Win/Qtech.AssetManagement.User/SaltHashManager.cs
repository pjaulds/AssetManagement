using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace Qtech.AssetManagement.User
{
    public static class SaltHashManager
    {
        #region --> Generate SALT Key  

        static int saltLengthLimit = 512;
        public static byte[] Get_SALT()
        {
            return Get_SALT(saltLengthLimit);
        }

        public static byte[] Get_SALT(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];

            //Require NameSpace: using System.Security.Cryptography;  
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        #endregion

        #region --> Generate HASH Using SHA512  
        public static string Get_HASH_SHA512(string password, string username, byte[] salt)
        {
            try
            {
                //required NameSpace: using System.Text;  
                //Plain Text in Byte  
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(password + username);

                //Plain Text + SALT Key in Byte  
                byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];

                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = salt[i];
                }

                HashAlgorithm hash = new SHA512Managed();
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = salt[i];
                }

                return Convert.ToBase64String(hashWithSaltBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region --> Comapare HASH Value  
        public static bool CompareHashValue(string password, string username, string OldHASHValue, byte[] SALT)
        {
            try
            {
                string expectedHashString = Get_HASH_SHA512(password, username, SALT);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}