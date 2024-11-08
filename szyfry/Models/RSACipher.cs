using System;
using System.Security.Cryptography;
using System.Text;

public class RSACipher
{
    private RSA rsa;

    public RSACipher()
    {
        rsa = RSA.Create();
        rsa.KeySize = 2048; // Rozmiar klucza (2048 bitów to bezpieczny rozmiar)
    }

    public string GeneratePublicKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPublicKey());
    }

    public string GeneratePrivateKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }

    public string Encrypt(string plaintext, string publicKey)
    {
        byte[] data = Encoding.UTF8.GetBytes(plaintext);
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        byte[] encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToBase64String(encryptedData);
    }

    public string Decrypt(string encryptedText, string privateKey)
    {
        byte[] encryptedData = Convert.FromBase64String(encryptedText);
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedData);
    }
}