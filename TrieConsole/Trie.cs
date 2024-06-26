using System.Collections.Generic;

namespace TrieConsole
{
    public class Trie
    {
        public SortedDictionary<char, Trie> DicAlphabet { get; set; }
        public int Count { get; set; }

        public Trie()
        {
            DicAlphabet = [];
            Count = 0;
        }
    }
}
