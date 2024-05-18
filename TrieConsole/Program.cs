using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace TrieConsole
{
    public static class Program
    {
        private static Trie _root = null;

        public static void Main()
        {
            ReadText();

            PrintOutWordCounts();
        }

        private static void ReadText()
        {
            string str = ReadFileText();

            if (!string.IsNullOrEmpty(str))
            {
                string[] arrStr = str.Split(" ");
                _root = new Trie();

                // insert nodes into trie
                foreach (string strSplit in arrStr)
                {
                    _root = InsertNodes(_root, new string(strSplit.Where(Char.IsLetter).ToArray()).ToLower());
                }

                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error:");
            Console.ResetColor();
            Console.WriteLine(" No text file loaded");
        }

        private static string ReadFileText()
        {
            string workingDirectory;
            string textFile;

            if (Debugger.IsAttached)
            {
                workingDirectory = Environment.CurrentDirectory;

                textFile = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\Files\book-war-and-peace.txt";
            }
            else
            {
                workingDirectory = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);

                textFile = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + "/Files/book-war-and-peace.txt";
            }

            StringBuilder sb = new();

            if (File.Exists(textFile))
            {
                using StreamReader sr = File.OpenText(textFile);
                string line;

                // Read line by line
                while ((line = sr.ReadLine()) != null)
                {
                    sb.Append(line + " ");
                }
            }

            sb.Replace("--", " ");

            sb.Replace("-", "");

            return sb.ToString();
        }

        private static Trie InsertNodes(Trie node, string str)
        {
            Trie currTrie = node;

            for (int i = 0; i < str.Length; i++)
            {
                if (!currTrie.DicAlphabet.TryGetValue(str[i], out Trie value))
                {
                    value = new Trie();

                    currTrie.DicAlphabet.Add(str[i], value);
                }

                currTrie = value;

                // increment count on leaf
                if (i == str.Length - 1)
                {
                    currTrie.Count++;
                }
            }

            return node;
        }

        private static void PrintOutWordCounts()
        {
            if (_root == null)
            {
                return;
            }

            Console.Clear();

            // get count of word occurrences in the trie
            ReadNodeCounts(_root, string.Empty, 1);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press return to exit");
            Console.ResetColor();

            Console.ReadLine();
        }

        private static int ReadNodeCounts(Trie node, string word, int printCount)
        {
            Trie nextTrie;

            // base case to print out count of word occurrences for leaf node
            if (node.DicAlphabet.Count == 0)
            {
                PrintWordCount(word, node.Count, printCount);

                printCount++;
            }
            else
            {
                // print word count for node
                if (node.Count >= 1)
                {
                    PrintWordCount(word, node.Count, printCount);

                    printCount++;
                }

                foreach (char currLetter in node.DicAlphabet.Keys)
                {
                    nextTrie = node.DicAlphabet[currLetter];

                    printCount = ReadNodeCounts(nextTrie, word + currLetter.ToString(), printCount);
                }
            }

            return printCount;
        }

        private static void PrintWordCount(string word, int wordCount, int printCount)
        {
            const int BATCH_SIZE = 300;
            const int COLS = 3;
            const int COL_WIDTH = 50;
            const int WORD_COUNT_MAX_ADD_TAB = 1000;

            string timesDisplayMsg = "times";
            string message;
            int padding;

            if (wordCount == 1)
            {
                timesDisplayMsg = "time";
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(word);
            Console.ResetColor();

            message = $" appears { wordCount } { timesDisplayMsg }";
            padding = COL_WIDTH - (word.Length + message.Length);

            if (printCount % COLS != 0 && padding > 0)
            {
                message = message.PadRight(padding);
            }

            if (printCount % COLS != 0 && wordCount > WORD_COUNT_MAX_ADD_TAB)
            {
                message += "\t";
            }

            if (printCount % COLS == 0)
            {
                Console.Write($"{ message }\n");
            }
            else
            {
                Console.Write($"{ message }\t");
            }

            if (printCount % BATCH_SIZE == 0)
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Press enter to view next batch or esc to exit");
                Console.ResetColor();

                ConsoleKeyInfo info = Console.ReadKey(true);

                if (info.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
