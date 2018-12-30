using System;
using System.Security.Cryptography;
using System.Text;

namespace GRS.Core.Services
{
   public interface IEncryptionService
   {
      string CreatePasswordHash(string password, string passwordSalt);

      bool VerifyHashedPassword(string hashedPassword, string providedPassword);
   }

   public class EncryptionService : IEncryptionService
   {
      private const string HaslAlgorithmSetting = "SHA512";

      /// <summary>
      /// Generate a hash for the given plain text values and returns a base64-encoded result.
      /// </summary>
      /// <param name="plainText">
      /// Plain text to be hashed. The function does not check if the provaided value is null
      /// </param>
      /// <param name="hashAlgorithm">
      /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", "SHA256", "SHA384" and
      /// "SHA512". (if any other value is specified, the "MD5" hashing algorithm will be used). This
      /// value is case insensitive
      /// </param>
      /// <param name="saltBytes">
      /// Salt Bytes. If the parameter is null, a random salt value will be generated
      /// </param>
      /// <returns>
      /// Returns the computed hash
      /// </returns>
      private static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
      {
         // Check salt and create a random if not specified
         if (saltBytes == null)
         {
            var minSaltSize = 4;
            var maxSaltSize = 8;

            var random = new Random();
            var saltSize = random.Next(minSaltSize, maxSaltSize);
            saltBytes = new byte[saltSize];

            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);
         }

         var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
         var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

         // copy plain text bytes
         for (var i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];

         // append the salt bytes
         for (var i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

         // determine the hash
         HashAlgorithm hash;

         // Make sure we have a hash algorithm specified
         if (hashAlgorithm == null)
            hashAlgorithm = string.Empty;

         // Size of hash is based on specified algorithm
         switch (hashAlgorithm.ToUpper())
         {
            case "SHA1":
               hash = new SHA1Managed();
               break;

            case "SHA256":
               hash = new SHA256Managed();
               break;

            case "SHA384":
               hash = new SHA384Managed();
               break;

            case "SHA512":
               hash = new SHA512Managed();
               break;

            default: // Must be MD5
               hash = new MD5CryptoServiceProvider();
               break;
         }

         // Now compute the hash
         var hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
         var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

         // copy hash to the hash result
         for (var i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

         // append the salt bytes to the hash result
         for (var i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

         // Convert result to base64-endoced string
         var hashValue = Convert.ToBase64String(hashWithSaltBytes);

         return hashValue;
      }

      /// <summary>
      /// Compares the hash of the specified plain text value to a given hash value. Plain text is
      /// hashed with the same salt value as the original hash.
      /// </summary>
      /// <param name="plainText">
      /// Plain text to be verified against the specified hash. The function does not check if the
      /// provaided value is null
      /// </param>
      /// <param name="hashAlgorithm">
      /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", "SHA256", "SHA384" and
      /// "SHA512". (if any other value is specified, the "MD5" hashing algorithm will be used). This
      /// value is case insensitive
      /// </param>
      /// <param name="hashValue">
      /// Base64-encoded hash value produced by ComputeHash function. This value includes the
      /// original salt appended to it.
      /// </param>
      /// <returns>
      /// Returns true if computed hash matches the specified hash, false otherwise
      /// </returns>
      private static bool VerifyHash(string plainText, string hashAlgorithm, string hashValue)
      {
         // Convert based64-encoded hash value into byte array
         var hashWithSaltBytes = Convert.FromBase64String(hashValue);

         // need to know the size of hash (without the salt)
         int hashSizeInBits;

         // Make sure we have a hash algorithm specified
         if (hashAlgorithm == null)
            hashAlgorithm = string.Empty;

         // Size of hash is based on specified algorithm
         switch (hashAlgorithm.ToUpper())
         {
            case "SHA1":
               hashSizeInBits = 160;
               break;

            case "SHA256":
               hashSizeInBits = 256;
               break;

            case "SHA384":
               hashSizeInBits = 384;
               break;

            case "SHA512":
               hashSizeInBits = 512;
               break;

            default: // Must be MD5
               hashSizeInBits = 128;
               break;
         }

         // Convert size of hash from bits to bytes;
         var hashSizeInBytes = (int)(hashSizeInBits / 8);

         // Make sure that the specified hash value is long enough
         if (hashWithSaltBytes.Length < hashSizeInBytes)
            return false;

         // Allocate array to hold original salt bytes retrieved from hash
         var saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

         // Copy salt from the end of the hash to the new array
         for (var i = 0; i < saltBytes.Length; i++)
            saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

         // Compute a new hash string
         var expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);

         // If the computed hash matches the specified has, the plain text value must be correct
         return hashValue == expectedHashString;
      }

      public string CreatePasswordHash(string password, string passwordSalt)
      {
         return ComputeHash(password.Trim(), HaslAlgorithmSetting, Encoding.UTF8.GetBytes(passwordSalt));
      }

      public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
      {
         return VerifyHash(providedPassword.Trim(), HaslAlgorithmSetting, hashedPassword);
      }
   }
}
