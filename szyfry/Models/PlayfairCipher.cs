using System;
using System.Text;

public class PlayfairCipher
{
    private char[,] keySquare;
    private const int Size = 5;

    public PlayfairCipher(string key)
    {
        keySquare = GenerateKeySquare(key);
    }

    private char[,] GenerateKeySquare(string key)
    {
        char[,] keySquare = new char[Size, Size];
        string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // J jest zazwyczaj pomijane
        StringBuilder keyString = new StringBuilder();

        foreach (char c in key.ToUpper())
        {
            if (char.IsLetter(c) && !keyString.ToString().Contains(c) && c != 'J')
                keyString.Append(c);
        }

        foreach (char c in alphabet)
        {
            if (!keyString.ToString().Contains(c))
                keyString.Append(c);
        }

        for (int i = 0, k = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                keySquare[i, j] = keyString[k++];
            }
        }

        return keySquare;
    }

    private (int, int) FindPosition(char letter)
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (keySquare[i, j] == letter)
                    return (i, j);
            }
        }
        return (-1, -1);
    }

    private string ProcessText(string text, bool encrypting = true)
    {
        StringBuilder processedText = new StringBuilder();

        for (int i = 0; i < text.Length; i += 2)
        {
            char first = text[i];
            char second = (i + 1 < text.Length) ? text[i + 1] : 'X';

            if (first == second)
            {
                second = 'X';
                i--;
            }

            (int row1, int col1) = FindPosition(first);
            (int row2, int col2) = FindPosition(second);

            if (row1 == row2)
            {
                col1 = (col1 + (encrypting ? 1 : -1) + Size) % Size;
                col2 = (col2 + (encrypting ? 1 : -1) + Size) % Size;
            }
            else if (col1 == col2)
            {
                row1 = (row1 + (encrypting ? 1 : -1) + Size) % Size;
                row2 = (row2 + (encrypting ? 1 : -1) + Size) % Size;
            }
            else
            {
                int temp = col1;
                col1 = col2;
                col2 = temp;
            }

            processedText.Append(keySquare[row1, col1]);
            processedText.Append(keySquare[row2, col2]);
        }

        return processedText.ToString();
    }

    public string Encrypt(string plaintext)
    {
        plaintext = plaintext.ToUpper().Replace("J", "I").Replace(" ", "");
        return ProcessText(plaintext, true);
    }

    public string Decrypt(string ciphertext)
    {
        ciphertext = ciphertext.ToUpper().Replace(" ", "");
        return ProcessText(ciphertext, false);
    }
}
