using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Services.Interfaces;

public interface IPasswordEncryptionService
{
    // MUST USE PBKDF2 KEY-DERIVATION TECHNIQUE
    // PLEASE USE ONLY ONE ENCRYPTION ALGORITHM,
    // SELECT THE ALGORITHM IN THE IMPLEMENTATION OF
    // THIS INTERFACE

    // https://en.wikipedia.org/wiki/PBKDF2
    // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes?view=net-8.0
    // https://code-maze.com/csharp-hashing-salting-passwords-best-practices/

    (string encryptedPassword, string salt) Hash(string password);
    bool Verify(string password, string passwordHash, string salt);
}