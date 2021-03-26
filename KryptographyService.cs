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
            int col = 0;
            bool started = false;

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

            //StringBuilder sb = new StringBuilder();
            //foreach (var list in a)
            //{
            //    int size = list.Count;
            //    int len = input.Length;
            //    list.RemoveRange(0,list.Count);
            //    sb.Append(input.Substring(0, size));
            //    input = input.Remove(0, size);
            //}
            //return sb.ToString();

            return sb.ToString();
        }

        //public string EncodeRail(string input, int key)
        //{
        //    List<char>[] a = new List<char>[key];

        //    for (int i = 0; i < key; i++)
        //    {
        //        a[i] = new List<char>();
        //    }

        //    for (int i = 0; i < key; i++)
        //    {
        //        a[i].Add(input[i]);
        //    }

        //    StringBuilder sb = new StringBuilder();

        //    int modulo = key + key - 2;

        //    int level = 0;

        //    int current_letter = 0;

        //    for (int j = 0;  j < key; j++)
        //    {
        //        if (j == key - 1)
        //        {
        //            level = 0;
        //        }
        //        int add = modulo + current_letter - level;
        //        while (add <= input.Length - 1)
        //        {
        //            if(add >= 0 && add <= input.Length - 1)
        //                a[j].Add(input[add]);
        //            add += modulo - level;
        //        }
        //        current_letter++;
        //        level += 2;
        //    }

        //    foreach(var list in a)
        //    {
        //        foreach(char aa in list)
        //        {
        //            sb.Append(aa);
        //        }

        //    }

        //    return sb.ToString();
        //}

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
                { // dodac if na #
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
                //while (matrix[row, column] != input[i])
                //{
                //    column++;
                //}
                //result.Append(matrix[0, column]);
            }

            return result.ToString();
        }
    }
}