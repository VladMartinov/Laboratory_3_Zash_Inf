using System.Numerics;
using System.Text;

public class GammaCipher
{
    public static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // 1. Исходные данные
        string lastName = "MARTYNOV";

        // 2. Расчет начального значения
        byte[] nameBytes = Encoding.GetEncoding("windows-1251").GetBytes(lastName);
        byte initialValue = 0;
        foreach (byte b in nameBytes)
        {
            initialValue ^= b;
        }

        Console.WriteLine($"Исходная фамилия: {lastName}");
        Console.WriteLine($"Начальное значение для гаммы: {initialValue} (десятичное)");

        // 3. Реализация LFSR
        Console.WriteLine("\nГенерация гаммы LFSR:");
        byte[] lfsrGamma = GenerateLFSRGamma(initialValue, 10);
        PrintGammaTable("LFSR", lfsrGamma);

        // 4. Реализация BBS
        Console.WriteLine("\nГенерация гаммы BBS:");
        BigInteger seed = new BigInteger(initialValue);
        BigInteger[] bbsGamma = GenerateBBSGamma(seed, 10);
        PrintGammaTable("BBS", bbsGamma);

        // 5. Шифрование и дешифрование (демонстрация)
        string encryptedText = Encrypt(lastName, bbsGamma);
        string decryptedText = Decrypt(encryptedText, bbsGamma);

        Console.WriteLine($"\nИсходный текст: {lastName}");
        Console.WriteLine($"Зашифрованный текст: {encryptedText}");
        Console.WriteLine($"Расшифрованный текст: {decryptedText}");
    }

    // 2.1. Метод для вычисления начального значения
    public static byte GetInitialValue(string name)
    {
        byte[] nameBytes = Encoding.GetEncoding("windows-1251").GetBytes(name);
        byte initialValue = 0;
        foreach (byte b in nameBytes)
        {
            initialValue ^= b;
        }
        return initialValue;
    }

    // 3. LFSR
    static byte[] GenerateLFSRGamma(byte initialValue, int count)
    {
        byte[] gamma = new byte[count];
        byte lfsr = initialValue;
        for (int i = 0; i < count; i++)
        {
            gamma[i] = lfsr;
            byte bit8 = (byte)((lfsr >> 7) & 1);
            byte bit4 = (byte)((lfsr >> 3) & 1);
            byte bit3 = (byte)((lfsr >> 2) & 1);
            byte bit2 = (byte)((lfsr >> 1) & 1);
            byte bit1 = (byte)(lfsr & 1);
            byte newBit = (byte)(bit8 ^ bit4 ^ bit3 ^ bit2 ^ bit1);
            lfsr = (byte)((lfsr << 1) | newBit);
        }
        return gamma;
    }

    // 4. BBS
    static BigInteger[] GenerateBBSGamma(BigInteger seed, int count)
    {
        BigInteger p = 79;  // Выберем простые p и q  что бы (p mod 4 == 3 ) (q mod 4 == 3)
        BigInteger q = 107;
        BigInteger n = p * q;
        BigInteger[] gamma = new BigInteger[count];
        for (int i = 0; i < count; i++)
        {
            seed = (seed * seed) % n;
            gamma[i] = seed;
        }
        return gamma;
    }

    // 5.1 Шифрование
    static string Encrypt(string text, BigInteger[] bbsGamma)
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(text);
        byte[] encryptedBytes = new byte[textBytes.Length];
        for (int i = 0; i < textBytes.Length; i++)
        {
            byte gammaByte = (byte)(bbsGamma[i % bbsGamma.Length] % 256);
            encryptedBytes[i] = (byte)(textBytes[i] ^ gammaByte);
        }
        return Convert.ToBase64String(encryptedBytes);
    }

    // 5.2 Дешифрование
    static string Decrypt(string encryptedText, BigInteger[] bbsGamma)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] decryptedBytes = new byte[encryptedBytes.Length];
        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            byte gammaByte = (byte)(bbsGamma[i % bbsGamma.Length] % 256);
            decryptedBytes[i] = (byte)(encryptedBytes[i] ^ gammaByte);
        }
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    // 6. Вывод таблиц
    static void PrintGammaTable(string generatorName, byte[] gamma)
    {
        Console.WriteLine($"Таблица гаммы ({generatorName}):");
        Console.WriteLine("Итерация\tЗначение (Dec)\tЗначение (Bin)");
        for (int i = 0; i < gamma.Length; i++)
        {
            Console.WriteLine($"{i + 1}\t\t{gamma[i]}\t\t{Convert.ToString(gamma[i], 2).PadLeft(8, '0')}");
        }
    }

    static void PrintGammaTable(string generatorName, BigInteger[] gamma)
    {
        Console.WriteLine($"Таблица гаммы ({generatorName}):");
        Console.WriteLine("Итерация\tЗначение (Dec)\tЗначение (Bin)");
        for (int i = 0; i < gamma.Length; i++)
        {
            Console.WriteLine($"{i + 1}\t\t{gamma[i]}\t\t{Convert.ToString((byte)(gamma[i] % 256), 2).PadLeft(8, '0')}");
        }
    }
}