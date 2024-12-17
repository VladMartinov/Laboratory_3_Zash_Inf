using System.Text;

namespace Laboratory_3_Zash_Inf
{
    public class Program
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static Dictionary<char, int> alphabetMap;

        static Program()
        {
            alphabetMap = new Dictionary<char, int>();
            for (int i = 0; i < Alphabet.Length; i++)
            {
                alphabetMap[Alphabet[i]] = i;
            }
        }

        // Шифрование по модулю N
        public static string EncryptModN(string message, string key)
        {
            StringBuilder encrypted = new StringBuilder();
            int keyIndex = 0;

            message = message.ToUpper();

            for (int i = 0; i < message.Length; i++)
            {
                if (!alphabetMap.ContainsKey(message[i]))
                {
                    encrypted.Append(message[i]);
                    continue;
                }
                int messageValue = alphabetMap[message[i]];
                int keyValue;
                if (keyIndex < key.Length)
                    keyValue = alphabetMap[key[keyIndex].ToString().ToUpper()[0]];
                else
                {
                    keyIndex = 0;
                    keyValue = alphabetMap[key[keyIndex].ToString().ToUpper()[0]];
                }

                int encryptedValue = (messageValue + keyValue) % Alphabet.Length;
                encrypted.Append(Alphabet[encryptedValue]);
                keyIndex++;

            }
            return encrypted.ToString();
        }

        // Дешифрование по модулю N
        public static string DecryptModN(string encryptedMessage, string key)
        {
            StringBuilder decrypted = new StringBuilder();
            int keyIndex = 0;
            for (int i = 0; i < encryptedMessage.Length; i++)
            {
                if (!alphabetMap.ContainsKey(encryptedMessage[i]))
                {
                    decrypted.Append(encryptedMessage[i]);
                    continue;
                }
                int encryptedValue = alphabetMap[encryptedMessage[i]];
                int keyValue;
                if (keyIndex < key.Length)
                    keyValue = alphabetMap[key[keyIndex].ToString().ToUpper()[0]];
                else
                {
                    keyIndex = 0;
                    keyValue = alphabetMap[key[keyIndex].ToString().ToUpper()[0]];
                }

                int decryptedValue = (encryptedValue + Alphabet.Length - keyValue) % Alphabet.Length;
                decrypted.Append(Alphabet[decryptedValue]);
                keyIndex++;

            }
            return decrypted.ToString();
        }

        // Шифрование по модулю 2 (XOR)
        public static string EncryptMod2(string message, string key)
        {
            StringBuilder encrypted = new StringBuilder();
            int keyIndex = 0;
            foreach (char messageChar in message)
            {
                int messageCode = (int)messageChar;

                int keyCode;
                if (keyIndex < key.Length)
                    keyCode = (int)key[keyIndex];
                else
                {
                    keyIndex = 0;
                    keyCode = (int)key[keyIndex];
                }

                int encryptedCode = messageCode ^ keyCode;
                encrypted.Append((char)encryptedCode);
                keyIndex++;

            }
            return encrypted.ToString();
        }

        // Дешифрование по модулю 2 (XOR)
        public static string DecryptMod2(string encryptedMessage, string key)
        {
            return EncryptMod2(encryptedMessage, key); // XOR is symmetric
        }

        // Вспомогательный метод для конвертации в двоичный вид
        public static string ToBinaryString(string input)
        {
            StringBuilder binaryString = new StringBuilder();
            foreach (char c in input)
            {
                binaryString.Append(Convert.ToString((int)c, 2).PadLeft(8, '0') + " ");
            }
            return binaryString.ToString().TrimEnd();
        }

        public static string ToDecimalString(string input)
        {
            StringBuilder binaryString = new StringBuilder();
            foreach (char c in input)
            {
                binaryString.Append(Convert.ToString((int)c, 10) + " ");
            }
            return binaryString.ToString().TrimEnd();
        }

        public static string ToTableDecimalString(string input)
        {
            StringBuilder binaryString = new StringBuilder();
            foreach (char c in input)
            {
                binaryString.Append(Alphabet.IndexOf($"{c}".ToUpper()) + " ");
            }
            return binaryString.ToString().TrimEnd();
        }

        public static void Main(string[] args)
        {
            // Использования шифра по модулю N
            string messageN = "MARTYNOV";
            string keyN = "SECRET";
            string encryptedN = EncryptModN(messageN, keyN);
            string decryptedN = DecryptModN(encryptedN, keyN);

            Console.WriteLine("Шифрование по модулю N:\n");
            Console.WriteLine($"Исходное сообщение: {messageN}");
            Console.WriteLine($"Исходное сообщение (dec): \n{ToTableDecimalString(messageN)}\n");
            Console.WriteLine($"Гамма: {keyN}");
            Console.WriteLine($"Гамма (dec): \n{ToTableDecimalString(keyN)}\n");
            Console.WriteLine($"Зашифрованное сообщение: {encryptedN}");
            Console.WriteLine($"Зашифрованное сообщение (dec): \n{ToTableDecimalString(encryptedN)}\n");
            Console.WriteLine($"Расшифрованное сообщение: {decryptedN}");
            Console.WriteLine();

            // Использования шифра по модулю 2
            string message2 = "MARTYNOV";
            string key2 = "PASSWORD";
            string encrypted2 = EncryptMod2(message2, key2);
            string decrypted2 = DecryptMod2(encrypted2, key2);

            Console.WriteLine("Шифрование по модулю 2:\n");
            Console.WriteLine($"Исходное сообщение: {message2}");
            Console.WriteLine($"Исходное сообщение (двоичный вид): \n{ToBinaryString(message2)}");
            Console.WriteLine($"Исходное сообщение (dec): \n{ToDecimalString(message2)}\n");
            Console.WriteLine($"Гамма: {key2}");
            Console.WriteLine($"Гамма (двоичный вид): \n{ToBinaryString(key2)}");
            Console.WriteLine($"Гамма (dec): \n{ToDecimalString(key2)}\n");
            Console.WriteLine($"Зашифрованное сообщение: {encrypted2}");
            Console.WriteLine($"Зашифрованное сообщение (двоичный вид): \n{ToBinaryString(encrypted2)}");
            Console.WriteLine($"Зашифрованное сообщение (dec): \n{ToDecimalString(encrypted2)}\n");
            Console.WriteLine($"Расшифрованное сообщение: {decrypted2}");
            Console.WriteLine();
        }
    }
}
