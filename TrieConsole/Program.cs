using System;
using System.Linq;

namespace TrieConsole
{
    public class Program
    {
        private static Trie _root;

        public static void Main(string[] args)
        {
            ReadText();

            PrintOutWordCounts();
        }

        private static void ReadText()
        {
            string str = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted. The wise man therefore always holds in these matters to this principle of selection: he rejects pleasures to secure other greater pleasures, or else he endures pains to avoid worse pains.";
            string[] arrStr = str.Split(" ");
            _root = new Trie();

            // insert nodes into trie 
            foreach (string strSplit in arrStr)
            {
                _root = InsertNodes(_root, new string(strSplit.Where(Char.IsLetter).ToArray()).ToLower());
            }
        }

        private static void PrintOutWordCounts()
        {
            // get count of word occurrences in the trie
            ReadNodeCounts(_root, string.Empty);
        }

        private static Trie InsertNodes(Trie node, string str)
        {
            Trie currTrie = node;

            for (int i = 0; i < str.Length; i++)
            {
                if (!currTrie.DicAlphabet.ContainsKey(str[i]))
                {
                    currTrie.DicAlphabet.Add(str[i], new Trie());
                }                

                currTrie = currTrie.DicAlphabet[str[i]];

                // increment count on leaf
                if (i == str.Length - 1)
                {
                    currTrie.Count++;
                }
            }

            return node;
        }

        private static void ReadNodeCounts(Trie node, string word)
        {
            Trie nextTrie;

            // base case to print out count of word occurrences
            if (node.DicAlphabet.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(word);
                Console.ResetColor();
                Console.Write($" appears { node.Count } times.\n");
            }
            else
            {
                foreach (char currLetter in node.DicAlphabet.Keys)
                {
                    nextTrie = node.DicAlphabet[currLetter];

                    ReadNodeCounts(nextTrie, (word + currLetter.ToString()));
                }
            }
        }
    }
}
