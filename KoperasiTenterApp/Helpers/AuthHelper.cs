using System.Text;

namespace KoperasiTenterApp.Helpers
{
    public static class AuthHelper
    {
        public static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            pinSalt = hmac.Key;
            pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
        }

        public static bool VerifyPin(string pin, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            return computedHash.SequenceEqual(storedHash);
        }

        public static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(1000, 9999).ToString();
        }
    }
}
