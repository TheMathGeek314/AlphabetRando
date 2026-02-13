namespace AlphabetRando {
    public class GlobalSettings {
        public bool Vowels = false;
        public bool DupeVowels = false;
        public bool Consonants = false;
        public bool DupeConsonants = false;

        public bool Any => Vowels || Consonants;
    }

    public class LocalSettings {
        public bool Vowels = false;
        public bool Consonants = false;
    }
}
