namespace SipPOS.Services.General.Interfaces;

/// <summary>
/// Service interface for password encryption using PBKDF2 key-derivation technique.
/// </summary>
public interface IPasswordEncryptionService
{
    // MUST USE PBKDF2 KEY-DERIVATION TECHNIQUE
    // PLEASE USE ONLY ONE ENCRYPTION ALGORITHM,
    // SELECT THE ALGORITHM IN THE IMPLEMENTATION OF
    // THIS INTERFACE

    // https://en.wikipedia.org/wiki/PBKDF2
    // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=net-8.0
    // https://code-maze.com/csharp-hashing-salting-passwords-best-practices/

    /// <summary>
    /// Hashes the specified password using PBKDF2.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the hashed password and the salt used.</returns>
    (string encryptedPassword, string salt) Hash(string password);

    /// <summary>
    /// Verifies the specified password against the given password hash and salt.
    /// </summary>
    /// <param name="password">The password to verify against the hashed password.</param>
    /// <param name="passwordHash">The hashed password to compare against.</param>
    /// <param name="salt">The salt used for hashing the password.</param>
    /// <returns>True if the password is equal with the password before hash; otherwise, false.</returns>
    bool Verify(string password, string passwordHash, string salt);
}