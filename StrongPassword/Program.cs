using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StrongPassword
{
    class Program
    {
        static void Main(string[] args)
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = GenerateRandomSalt(rng, 16);
            var sha1 = ComputeSaltedHash("admin", salt);
            Console.WriteLine("Salt: " + salt);
            Console.WriteLine("Password: " + sha1);
        }

        public static string GenerateRandomSalt(RNGCryptoServiceProvider rng, int size)
        {
            var bytes = new Byte[size];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string ComputeSaltedHash(string password, string salt)
        {
            // Create Byte array of password string
            var encoder = new ASCIIEncoding();
            var saltAndPassword = string.Concat(password, salt);
            var secretBytes = encoder.GetBytes(saltAndPassword);

            // Create a new salt
            var saltBytes = new Byte[4];
            var salts = salt.Length;
            saltBytes[0] = (byte)(salts >> 24);
            saltBytes[1] = (byte)(salts >> 16);
            saltBytes[2] = (byte)(salts >> 8);
            saltBytes[3] = (byte)(salts);

            // append the two arrays
            var toHash = new Byte[secretBytes.Length + saltBytes.Length];
            Array.Copy(secretBytes, 0, toHash, 0, secretBytes.Length);
            Array.Copy(saltBytes, 0, toHash, secretBytes.Length, saltBytes.Length);

            var sha1 = SHA1.Create();
            var computedHash = sha1.ComputeHash(toHash);

            return Convert.ToBase64String(computedHash);

            //return Convert.FromBase64String(computedHash);
            //return encoder.GetString(computedHash);
        }
    }
}
