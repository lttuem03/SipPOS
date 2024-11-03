using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Services.Interfaces;

namespace SipPOS.Services.Implementations;

public class PasswordEncryptionService : IPasswordEncryptionService
{
    // ref: https://code-maze.com/csharp-hashing-salting-passwords-best-practices/
    const int SALT_SIZE = 32;  // 32 bytes for 64 hex characters
    const int HASH_SIZE = 32;  // 32 bytes for 64 hex characters
    const int ITERATIONS = 300000;
    static readonly HashAlgorithmName HASH_ALGORITHM = HashAlgorithmName.SHA256;

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
