using System;
using System.Collections.Generic;
using System.Text;

namespace Krypto
{
    public class KryptographyService
    {
        public string DecodeRail(string input, int key)
        {
            List<char>[] a = new List<char>[key];

            for (int i = 0; i < key; i++)
            {
                a[i] = new List<char>();
            }

            int level = 0;
            bool direction = false; //true going down

            for (int i = 0; i < input.Length; i++)
            {
                if (level == 0 || level == key - 1)
                    direction = !direction;

                a[level].Add('#');

                if (direction)
                    level++;
                else
                    level--;
            }

            int index = 0;
            for (int i = 0; i < key; i++)
                for (int j = 0; j < input.Length; j++)
                    if (a[i].Contains('#') && index < input.Length)
                        a[i][j] = input[index++];

            level = 0;
            direction = false;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (level == 0 || level == key - 1)
                {
                    direction = !direction;
                }

                sb.Append(a[level][0]);
                a[level].RemoveAt(0);

                if (direction)
                    level++;
                else
                    level--;
            }

            return sb.ToString();
        }

        public string EncodeRail(string input, int key)
        {
            List<char>[] a = new List<char>[key];

            for (int i = 0; i < key; i++)
            {
                a[i] = new List<char>();
            }

            int level = 0;
            bool direction = false; //true going down

            for (int i = 0; i < input.Length; i++)
            {
                if (level == 0 || level == key - 1)
                    direction = !direction;

                a[level].Add(input[i]);

                if (direction)
                    level++;
                else
                    level--;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var list in a)
            {
                foreach (char aa in list)
                {
                    sb.Append(aa);
                }
            }
            return sb.ToString();
        }

        public string EncodeMatrixShift(string input)
        {
            int columns = 5;
            int rows = input.Length % columns == 0 ? (input.Length / columns) : (input.Length / columns) + 1;

            char[,] matrix = new char[rows, columns];

            for (int i = 0, k = 0; i < rows; i++)
            {
                for (int j = 0; j < columns;)
                {
                    if (k <= input.Length - 1)
                    {
                        matrix[i, j] = input[k];
                    }
                    j++;
                    k++;
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                // key 3-4-1-5-2
                sb.Append(matrix[i, 3 - 1]);
                sb.Append(matrix[i, 4 - 1]);
                sb.Append(matrix[i, 1 - 1]);
                sb.Append(matrix[i, 5 - 1]);
                sb.Append(matrix[i, 2 - 1]);
            }

            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string DecodeMatrixShift(string input)
        {
            int columns = 5;
            int rows = input.Length % columns == 0 ? (input.Length / columns) : (input.Length / columns) + 1;
            char[,] matrix = new char[rows, columns];

            for (int i = 0, k = 0; i < rows; i++)
            {
                for (int j = 0; j < columns;)
                {
                    if (k <= input.Length - 1)
                        matrix[i, j] = '#';
                    j++;
                    k++;
                }
            }

            int letter = 0;
            for (int i = 0; i < rows; i++)
            {
                try
                {
                    if (letter < input.Length - 1)
                    {
                        if (matrix[i, 3 - 1] == '#')
                            matrix[i, 3 - 1] = input[letter++];
                        if (matrix[i, 4 - 1] == '#')
                            matrix[i, 4 - 1] = input[letter++];
                        if (matrix[i, 1 - 1] == '#')
                            matrix[i, 1 - 1] = input[letter++];
                        if (matrix[i, 5 - 1] == '#')
                            matrix[i, 5 - 1] = input[letter++];
                        if (matrix[i, 2 - 1] == '#')
                            matrix[i, 2 - 1] = input[letter++];
                    }
                }
                catch (System.Exception)
                {
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sb.Append(matrix[i, j]);
                }
            }
            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string Encode2b(string input, string key)
        {
            char[] key_word_original_order = key.ToCharArray();
            char[] key_word_sorted = key.ToCharArray();
            Array.Sort(key_word_sorted);

            List<KeyValuePair<char, int>> letter_order = new List<KeyValuePair<char, int>>();

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key_word_original_order[i] == key_word_sorted[j])
                    {
                        letter_order.Add(new KeyValuePair<char, int>(key_word_original_order[i], j));
                        key_word_sorted[j] = '#';
                        break;
                    }
                }
            }

            int columns = key.Length;
            int rows = input.Length % columns == 0 ? (input.Length / columns) : (input.Length / columns) + 1;

            char[,] matrix = new char[rows, columns];

            for (int i = 0, k = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (k <= input.Length - 1)
                    {
                        matrix[i, j] = input[k];
                    }
                    k++;
                }
            }

            // odczyt

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < columns; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                int column_to_take = letter_order.LastIndexOf(temp);
                for (int j = 0; j < rows; j++)
                {
                    sb.Append(matrix[j, column_to_take]);
                    //0 9 6 10 2 7 5 3 8 1 4
                }
            }

            // RESULT PRZEZ CZYTANIE KOLUMN
            // TYLKO WIELKIE LITERY I BEZ SPACJI
            //CYPTRHGYOARP
            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string Decode2b(string input, string key)
        {
            char[] key_word_original_order = key.ToCharArray();
            char[] key_word_sorted = key.ToCharArray();
            Array.Sort(key_word_sorted);

            List<KeyValuePair<char, int>> letter_order = new List<KeyValuePair<char, int>>();

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key_word_original_order[i] == key_word_sorted[j])
                    {
                        letter_order.Add(new KeyValuePair<char, int>(key_word_original_order[i], j));
                        key_word_sorted[j] = '#';
                        break;
                    }
                }
            }

            int columns = key.Length;
            int rows = input.Length % columns == 0 ? (input.Length / columns) : (input.Length / columns) + 1;
            char[,] matrix = new char[rows, columns];

            for (int i = 0, k = 0; i < rows; i++)
            {
                for (int j = 0; j < columns;)
                {
                    if (k <= input.Length - 1)
                        matrix[i, j] = '#';
                    j++;
                    k++;
                }
            }
            int column_to_take = 0;

            //0 9 6 10 2 7 5 3 8 1 4
            for (int i = 0, k = 0; i < columns; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                column_to_take = letter_order.LastIndexOf(temp);
                for (int j = 0; j < rows; j++)
                {
                    if (matrix[j, column_to_take] == '#' && k <= input.Length - 1)
                    {
                        matrix[j, column_to_take] = input[k];
                        k++;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    sb.Append(matrix[i, j]);
                }
            }

            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string Encode2c(string input, string key)
        {
            char[] key_word_original_order = key.ToCharArray();
            char[] key_word_sorted = key.ToCharArray();
            Array.Sort(key_word_sorted);

            List<KeyValuePair<char, int>> letter_order = new List<KeyValuePair<char, int>>();

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key_word_original_order[i] == key_word_sorted[j])
                    {
                        letter_order.Add(new KeyValuePair<char, int>(key_word_original_order[i], j));
                        key_word_sorted[j] = '#';
                        break;
                    }
                }
            }

            int columns = key.Length;
            int rows = key.Length;
            char[,] matrix = new char[rows, columns];

            int letters_to_input = 0;
            for (int i = 0, k = 0; i < rows; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                letters_to_input = letter_order.LastIndexOf(temp) + 1;
                for (int j = 0; j < letters_to_input; j++)
                {
                    if (k <= input.Length - 1)
                    {
                        matrix[i, j] = input[k];
                        k++;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < columns; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                int column_to_take = letter_order.LastIndexOf(temp);
                for (int j = 0; j < rows; j++)
                {
                    sb.Append(matrix[j, column_to_take]);
                    //0 9 6 10 2 7 5 3 8 1 4
                }
            }
            //CRYHOARPGPYT
            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string Decode2c(string input, string key)
        {
            char[] key_word_original_order = key.ToCharArray();
            char[] key_word_sorted = key.ToCharArray();
            Array.Sort(key_word_sorted);

            List<KeyValuePair<char, int>> letter_order = new List<KeyValuePair<char, int>>();

            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (key_word_original_order[i] == key_word_sorted[j])
                    {
                        letter_order.Add(new KeyValuePair<char, int>(key_word_original_order[i], j));
                        key_word_sorted[j] = '#';
                        break;
                    }
                }
            }

            int columns = key.Length;
            int rows = key.Length;
            char[,] matrix = new char[rows, columns];

            int letters_to_input = 0;
            for (int i = 0, k = 0; i < rows; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                letters_to_input = letter_order.LastIndexOf(temp) + 1;
                for (int j = 0; j < letters_to_input; j++)
                {
                    if (k <= input.Length - 1)
                    {
                        matrix[i, j] = '#';
                        k++;
                    }
                }
            }

            int column_to_take = 0;
            for (int i = 0, k = 0; i < columns; i++)
            {
                KeyValuePair<char, int> temp = letter_order.Find(x => x.Value == i);
                column_to_take = letter_order.LastIndexOf(temp);
                for (int j = 0; j < rows; j++)
                {
                    if (matrix[j, column_to_take] == '#' && k <= input.Length - 1)
                    {
                        matrix[j, column_to_take] = input[k];
                        k++;
                    }
                    //0 9 6 10 2 7 5 3 8 1 4
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0, k = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (k <= input.Length - 1)
                    {
                        sb.Append(matrix[i, j]);
                    }
                }
            }
            //CRYHOARPGPYT
            //CRYHOARPGPYT
            sb.Replace("\0", string.Empty);
            return sb.ToString();
        }

        public string EncodeCeaser(string input, int a, int b)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char ch = (char)(((int)input[i] * a + b - 65) % 26 + 65);
                result.Append(ch);
            }
            return result.ToString();
        }

        public string DecodeCeaser(string input, int a, int b)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char ch = ((char)(((((int)input[i] + (26 - b) - 65) * (Math.Pow(a, 12 - 1))) % 26 + 65)));
                result.Append(ch);
            }
            return result.ToString();
        }

        public string EncodeVigenere(string input, string key)
        {
            int alphabet = 26;
            char[,] matrix = new char[alphabet, alphabet];
            int letter = 0;

            for (int i = 0; i < alphabet; i++)
            {
                for (int j = 0; j < alphabet; j++)
                {
                    if (letter < 26)
                    {
                        matrix[i, j] = (char)(65 + letter);
                        letter++;
                    }
                    else
                    {
                        letter = 0;
                        matrix[i, j] = (char)(65 + letter);
                        letter++;
                    }
                }
                letter = (letter + 1) % 26;
            }

            StringBuilder result = new StringBuilder();
            int column = 0, row = 0;
            // DOPASOWANIE KLUCZA MODULO
            for (int i = 0; i < input.Length; i++)
            {
                row = (int)key[i % key.Length] - 65;
                for (int j = 0; j < alphabet; j++)
                {
                    if (input[i] == matrix[0, j])
                        column = (int)matrix[0, j] - 65;
                }

                result.Append(matrix[row, column]);
            }

            return result.ToString();
        }

        public string DecodeVigenere(string input, string key)
        {
            int alphabet = 26;
            char[,] matrix = new char[alphabet, alphabet];
            int letter = 0;

            for (int i = 0; i < alphabet; i++)
            {
                for (int j = 0; j < alphabet; j++)
                {
                    if (letter < 26)
                    {
                        matrix[i, j] = (char)(65 + letter);
                        letter++;
                    }
                    else
                    {
                        letter = 0;
                        matrix[i, j] = (char)(65 + letter);
                        letter++;
                    }
                }
                letter = (letter + 1) % 26;
            }

            StringBuilder result = new StringBuilder();
            int column = 0, row = 0;

            for (int i = 0; i < input.Length; i++)
            {
                row = (int)key[i % key.Length] - 65;

                for (int j = 0; j < alphabet; j++)
                {
                    if (input[i] == matrix[row, j])
                    {
                        column = j;
                        break;
                    }
                }

                result.Append(matrix[0, column]);
            }

            return result.ToString();
        }

        public string LFSR(string seed, string mask, int length)
        {
            // seed wartosc wejsciowa
            // maska określa xorowanie
            // length dlugosc wyjsciowego ciagu bitow

            int rows = length + 1;
            int columns = seed.Length;
            char[,] matrix = new char[rows, columns];
            char XOR_value;

            for (int i = 0; i < columns; i++)
            {
                matrix[0, i] = seed[i]; // uzupełnienie pierwszego wiersza
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i + 1 < rows)
                    {
                        //przepisanie wartosci z przesunieciem bitu
                        //xorowanie zgodnie z wielomianem
                        if (j == 0) // wielomian na Q1
                        {
                            StringBuilder row = new StringBuilder();
                            // stworzenie wiersza do podania
                            for (int z = 0; z < columns; z++)
                            {
                                row.Append(matrix[i, z]);
                            }
                            XOR_value = XOR(row.ToString(), mask);
                            matrix[i + 1, 0] = XOR_value;
                            continue;
                        }

                        matrix[i + 1, j] = matrix[i, j - 1];
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            // odczyt wartosci z ostatniej kolumny
            for (int i = 1; i < rows; i++)
            {
                sb.Append(matrix[i, 0]); // wybor kolum
            }
            return sb.ToString(); ;
        }

        public char XOR(string seed, string mask)
        {
            char[] seedArr = seed.ToCharArray();
            char[] maskArr = mask.ToCharArray();
            int XOR = 0;

            for (int i = 0; i < seed.Length; i++)
            {
                if (maskArr[i] == '1' && seedArr[i] == '1')
                    XOR++;
            }

            if (XOR % 2 == 0)
                return '0'; // parzysta liczba jedynek
            else
                return '1'; // np liczba jedynek
        }

        public char XOR(string seed, string mask, string input)
        {
            char[] seedArr = seed.ToCharArray();
            char[] maskArr = mask.ToCharArray();
            char[] inputArr = input.ToCharArray();
            int XOR = 0;

            for (int i = 0; i < seed.Length; i++)
            {
                if (maskArr[i] == '1' && seedArr[i] == '1' && inputArr[i] == '1')
                    XOR++;
            }

            if (XOR % 2 == 0)
                return '0'; // parzysta liczba jedynek
            else
                return '1'; // np liczba jedynek
        }

        public char Single_XOR(char byte_1, char byte_2)
        {
            return byte_1 != byte_2 ? '1' : '0';
        }

        public string Multiple_XOR(string s1, string s2)
        {
            if (s1.Length != s2.Length)
                return "";

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s1.Length; i++)
            {
                sb.Append(Single_XOR(s1[i], s2[i]));
            }
            return sb.ToString();
        }

        public string Synchronous_Stream_Cipher(string input, string seed, string mask) // czytanie wartosci bez seed, czyli od 1 indexu wiersza
        {
            StringBuilder sb = new StringBuilder();
            string lsfr = LFSR(seed, mask, input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                sb.Append(Single_XOR(input[i], lsfr[i]));
            }
            return sb.ToString();
        }

        public string Ciphertext_Autokey_Encrypt(string input, string seed, string mask)
        {
            int rows = input.Length + 1;
            int columns = seed.Length;
            char[,] matrix = new char[rows, columns];
            char XOR_value;

            for (int i = 0; i < columns; i++)
            {
                matrix[0, i] = seed[i]; // uzupełnienie pierwszego wiersza
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i + 1 < rows)
                    {
                        //przepisanie wartosci z przesunieciem bitu
                        //xorowanie zgodnie z wielomianem
                        if (j == 0) // wielomian na Q1
                        {
                            StringBuilder row = new StringBuilder();
                            // stworzenie wiersza do podania
                            for (int z = 0; z < columns; z++)
                            {
                                row.Append(matrix[i, z]);
                            }
                            XOR_value = XOR(row.ToString(), mask);
                            XOR_value = Single_XOR(XOR_value, input[i]);
                            matrix[i + 1, 0] = XOR_value;
                            continue;
                        }

                        matrix[i + 1, j] = matrix[i, j - 1];
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            // odczyt wartosci z ostatniej kolumny
            for (int i = 1; i < rows; i++)
            {
                sb.Append(matrix[i, 0]); // wybor kolum
            }
            return sb.ToString();
        }

        public string Ciphertext_Autokey_Decrypt(string input, string seed, string mask) // branie wartosci od 0
        {
            int rows = input.Length;
            int columns = seed.Length;
            char[,] matrix = new char[rows, columns];

            for (int i = 0; i < columns; i++)
            {
                matrix[0, i] = seed[i]; // uzupełnienie pierwszego wiersza
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i + 1 < rows)
                    {
                        //przepisanie wartosci z przesunieciem bitu
                        //xorowanie zgodnie z wielomianem
                        if (j == 0) // wielomian na Q1
                        {
                            StringBuilder row = new StringBuilder();
                            // stworzenie wiersza do podania
                            for (int z = 0; z < columns; z++)
                            {
                                row.Append(matrix[i, z]);
                            }
                            // kolejna wartosc w kolumnie Q1 to bity ze słowa wejściowego
                            matrix[i + 1, 0] = input[i];
                            continue;
                        }

                        matrix[i + 1, j] = matrix[i, j - 1];
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder output = new StringBuilder();
            char XOR_value;
            for (int i = 0; i < rows; i++) // xor z maska
            {
                for (int z = 0; z < columns; z++)
                {
                    sb.Append(matrix[i, z]);
                }
                XOR_value = XOR(sb.ToString(), mask); // wybor kolumn
                output.Append(Single_XOR(XOR_value, input[i]));
                sb.Clear();
            }

            return output.ToString();
        }

        public string DES(string input, string key)
        {
            string[] subkeys = prepareDesKey(key); // KEY

            int numberOfblocks = input.Length % 64 == 0 ? (input.Length / 64) : (input.Length / 64) + 1; // jezeli uda sie podzielic na bloki po 64

            List<string> blocks = new List<string>();
            for (int i = 0; i < numberOfblocks; i++) // cut input into 64 blocks
            {
                string block = input.Substring(0, 64);
                input = input.Remove(0, 64);
                blocks.Add(block);
            }
            // initial permutation
            string check = initialPermatuation(blocks[0]);
            // break into 2
            string keyL = key.Substring(0, 32);
            keyL = permutedExpansion(keyL);
            string keyP = key.Substring(32, 32);
            keyP = permutedExpansion(keyP);
            keyP = Multiple_XOR(keyP, subkeys[0]); // XOR with key
            for (int i = 0; i < 8; i++) // break Right key into 8 x 6 bits
            {
                string subBlock = keyP.Substring(i * 6, 6);
                readTableData(subBlock, 0);
            }

            return "";
        }

        private int readTableData(string bitValue, int s)
        {
            int row = Convert.ToInt32(bitValue[0].ToString() + bitValue[bitValue.Length - 1], 2); // 0 and 5th bit -> int row
            int column = Convert.ToInt32(bitValue.Substring(1, 4), 2); // 1 2 3 4 bits -> int column

            int[,] dataTables =
                {
            {
            14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
            0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
            4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
            15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13,
            },
            {
            15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
            3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
            0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
            13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9,
            },
            {
            10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
            13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
            13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
            1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12,
            },
            {
            7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
            13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
            10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
            3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14,
            },
            {
            2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
            14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
            4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
            11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3,
            },
            {
            12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
            10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
            9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
            4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13,
            },
            {
            4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
            13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
            1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
            6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12,
            },
            {
            13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
            1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
            7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
            2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            };

            return 0;
        }

        private string[] prepareDesKey(string key)
        {
            key = permutedChoice_1(key); // initial permutation
            int[] ls = new int[] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            string[] subkeys = new string[ls.Length];
            for (int i = 0; i < ls.Length; i++)
            {
                // break key into 2 parts
                string keyC = key.Substring(0, 28);
                string keyD = key.Substring(28, 28);
                // do shifts & merge key
                key = leftShift(keyC, ls[i]) + leftShift(keyD, ls[i]);
                // do pc2
                subkeys[i] = permutedChoice_2(key);
            }
            return subkeys;
        }

        private string leftShift(string input, int shift)
        {
            string cut = input.Substring(0, shift);
            input = input.Remove(0, shift);
            input += cut;
            return input;
        }

        private string permutedChoice_1(string key) // key permutation, length reduction
        {
            int[] ip = new int[] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26,
                                   18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52,
                                   44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46,
                                   38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5,
                                   28, 20, 12, 4 };

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ip.Length; i++) // reduce key from 64 to 56 bits
            {
                sb.Append(key[ip[i] - 1]);
            }
            return sb.ToString();
        }

        private string permutedChoice_2(string key) // key permutation, length reduction
        {
            int[] ip = new int[] { 14,17,11,24,1,5,3,28,15,6,21,10,
                                   23,19,12,4,26,8,16,7,27,20,13,2,
                                   41,52,31,37,47,55,30,40,51,45,33,48,
                                   44,49,39,56,34,53,46,42,50,36,29,32};

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ip.Length; i++) // reduce key from 64 to 56 bits
            {
                sb.Append(key[ip[i] - 1]);
            }
            return sb.ToString();
        }

        private string initialPermatuation(string key)
        {
            int[] ip = new int[] {58,50,42,34,26,18,10,2,60,52,44,36,28,20,12,4,62,54,
                                  46,38,30,22,14,6,64,56,48,40,32,24,16,8,57,49,41,33,
                                  25,17,9,1,59,51,43,35,27,19,11,3,61,53,45,37,29,21,
                                  13,5,63,55,47,39,31,23,15,7};

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ip.Length; i++)
            {
                sb.Append(key[ip[i] - 1]);
            }
            return sb.ToString();
        }

        private string permutedExpansion(string key) // key permutation, length reduction
        {
            int[] ip = new int[] { 32,1,2,3,4,5,4,5,6,7,8,9,
                                   8,9,10,11,12,13,12,13,14,15,16,17,
                                   16,17,18,19,20,21,20,21,22,23,24,25,
                                   24,25,26,27,28,29,28,29,30,31,32,1};

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ip.Length; i++) // reduce key from 64 to 56 bits
            {
                sb.Append(key[ip[i] - 1]);
            }
            return sb.ToString();
        }
    }
}