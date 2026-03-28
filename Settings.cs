using Newtonsoft.Json;
using MenuChanger.Attributes;

namespace AlphabetRando {
    public class GlobalSettings {
        public bool Vowels = false;
        public bool Consonants = false;
        public bool Numbers = false;
        public bool Custom = false;

        public bool DupeVowels = false;
        public bool DupeConsonants = false;
        public bool DupeNumbers = false;
        public bool DupeCustom = false;

        [MenuIgnore]
        public string CustomCharacters = "";

        [JsonIgnore]
        public bool Any => Vowels || Consonants || Numbers || (Custom && CustomCharacters.Length > 0);
    }

    public class LocalSettings {
        public bool Vowels = false;
        public bool Consonants = false;
        public bool Numbers = false;
        public bool Custom = false;
        public string CustomCharacters = "";
    }
}
