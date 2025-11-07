using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;


namespace CREEMESTUDIO
{
    public static class Security
    {
        // ✅ Generate a random salt
        public static byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // ✅ Hash password with salt → returns lowercase hex string
        public static string HashPassword(string password, byte[] salt)
        {
            using (var sha = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password ?? "");
                byte[] combined = new byte[salt.Length + passwordBytes.Length];

                Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, combined, salt.Length, passwordBytes.Length);

                byte[] hash = sha.ComputeHash(combined);

                // Convert hash to hex string
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        // ✅ Verify entered password
        public static bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
        {
            string enteredHash = HashPassword(enteredPassword, storedSalt);
            return enteredHash == storedHash;
        }

        internal static string HashPassword(string p1)
        {
            throw new NotImplementedException();
        }
    }
}