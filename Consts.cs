using System.Collections.Generic;
using System.Linq;

namespace AlphabetRando {
    public static class Consts {
        public static readonly string[] vowels = ["A", "E", "I", "O", "U"];
        public static readonly string[] consonants = ["B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z"];

        public static IEnumerable<string> allLetters => vowels.Concat(consonants);
        public static List<string> itemNames => allLetters.Select(letter => "Alphabet-" + letter).ToList();
    }
}
