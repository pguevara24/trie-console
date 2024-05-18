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
            
            Console.WriteLine();
            Console.WriteLine("Press return to exit");

            // wait for user input
            Console.ReadLine();
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

            // get count of word occurrences in the trie
            ReadNodeCounts(_root, string.Empty);
        }

        private static void ReadNodeCounts(Trie node, string word)
        {
            Trie nextTrie;

            // base case to print out count of word occurrences for leaf node
            if (node.DicAlphabet.Count == 0)
            {
                PrintWordCount(word, node.Count);
            }
            else
            {
                // print word count for node
                if (node.Count >= 1)
                {
                    PrintWordCount(word, node.Count);
                }

                foreach (char currLetter in node.DicAlphabet.Keys)
                {
                    nextTrie = node.DicAlphabet[currLetter];

                    ReadNodeCounts(nextTrie, word + currLetter.ToString());
                }
            }
        }

        private static void PrintWordCount(string word, int wordCount)
        {
            string timesDisplayMsg = "times";

            if (wordCount == 1)
            {
                timesDisplayMsg = "time";
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(word);
            Console.ResetColor();
            Console.Write($" appears { wordCount } { timesDisplayMsg }.\n");
        }
    }
}
