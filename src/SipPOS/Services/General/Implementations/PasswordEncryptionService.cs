using System.Text;
using System.Security.Cryptography;

using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.General.Implementations;

/// <summary>
/// Service for password encryption and verification.
/// </summary>
public class PasswordEncryptionService : IPasswordEncryptionService
{
    // ref: https://code-maze.com/csharp-hashing-salting-passwords-best-practices/
    const int SALT_SIZE = 32;  // 32 bytes for 64 hex characters
    const int HASH_SIZE = 32;  // 32 bytes for 64 hex characters
    const int ITERATIONS = 300000;
    static readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA256;

    /// <summary>
    /// Hashes the specified password using a generated salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the hashed password and the salt.</returns>
    public (string encryptedPassword, string salt) Hash(string password)
    {
        var saltAsBytes = RandomNumberGenerator.GetBytes(SALT_SIZE);

        var passwordHashAsBytes = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: saltAsBytes,
            iterations: ITERATIONS,
            hashAlgorithm: HASH_ALGORITHM,
            outputLength: HASH_SIZE
        );

        return (
            encryptedPassword: Convert.ToHexString(passwordHashAsBytes),
            salt: Convert.ToHexString(saltAsBytes)
        );
    }

    /// <summary>
    /// Verifies the specified password against the given hash and salt.
    /// </summary>
    /// <param name="password">The password to verify against the hashed password.</param>
    /// <param name="passwordHash">The hashed password.</param>
    /// <param name="salt">The salt used to hash the password.</param>
    /// <returns>True if the password is the right password before hashing; otherwise, false.</returns>
    public bool Verify(string password, string passwordHash, string salt)
    {
        var verifyingPasswordHashAsBytes = Rfc2898DeriveBytes.Pbkdf2(
            password: Encoding.UTF8.GetBytes(password),
            salt: Convert.FromHexString(salt),
            iterations: ITERATIONS,
            hashAlgorithm: HASH_ALGORITHM,
            outputLength: HASH_SIZE
        );

        return CryptographicOperations.FixedTimeEquals(
            left: verifyingPasswordHashAsBytes,
            right: Convert.FromHexString(passwordHash)
        );
    }
}
