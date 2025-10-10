using System;
using System.Security.Cryptography;
using Atria.Application.Common.Interfaces;

namespace Atria.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bits
    private const int KeySize = 32; // 256 bits
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            KeySize);

        return string.Join(
            ":",
            Convert.ToBase64String(salt),
            Iterations,
            Algorithm.Name,
            Convert.ToBase64String(hash));
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        string[] parts = hashedPassword.Split(':');
        if (parts.Length != 4)
        {
            return false;
        }

        byte[] salt = Convert.FromBase64String(parts[0]);
        int iterations = int.Parse(parts[1]);
        HashAlgorithmName algorithm = new(parts[2]);
        byte[] hash = Convert.FromBase64String(parts[3]);

        byte[] providedHash = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            iterations,
            algorithm,
            hash.Length);

        return CryptographicOperations.FixedTimeEquals(hash, providedHash);
    }
}