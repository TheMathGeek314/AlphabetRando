using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace AlphabetRando {
    public class RequestModifier {
        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
            RequestBuilder.OnUpdate.Subscribe(-499.5f, DefinePools);
            RequestBuilder.OnUpdate.Subscribe(0, CloneSettings);
        }

        private static void SetupItems(RequestBuilder rb) {
            if(!AlphabetRando.globalSettings.Any)
                return;

            GlobalSettings gs = AlphabetRando.globalSettings;
            foreach(string letter in Consts.itemNames) {
                rb.EditItemRequest(letter, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = letter,
                        Pool = "Alphabet",
                        MajorItem = false,
                        PriceCap = 26
                    };
                });
            }
            foreach(char letter in AlphabetRando.globalSettings.CustomCharacters) {
                string name = "Alphabet-" + letter;
                rb.EditItemRequest(name, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = name,
                        Pool = "Alphabet",
                        MajorItem = false,
                        PriceCap = 26
                    };
                });
            }
            if(gs.Vowels) {
                foreach(string vowel in Consts.vowels) {
                    rb.AddItemByName("Alphabet-" + vowel, gs.DupeVowels ? 2 : 1);
                }
            }
            if(gs.Consonants) {
                foreach(string consonant in Consts.consonants) {
                    rb.AddItemByName("Alphabet-" + consonant, gs.DupeConsonants ? 2 : 1);
                }
            }
            if(gs.Numbers) {
                foreach(string number in Consts.numbers) {
                    rb.AddItemByName("Alphabet-" + number, gs.DupeNumbers ? 2 : 1);
                }
            }
            if(gs.Custom) {
                foreach(char letter in AlphabetRando.globalSettings.CustomCharacters) {
                    rb.AddItemByName("Alphabet-" + letter, gs.DupeCustom ? 2 : 1);
                }
            }
        }

        private static void DefinePools(RequestBuilder rb) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            ItemGroupBuilder group = null;
            string label = RBConsts.SplitGroupPrefix + "Alphabet";
            foreach(ItemGroupBuilder igb in rb.EnumerateItemGroups()) {
                if(igb.label == label) {
                    group = igb;
                    break;
                }
            }
            group ??= rb.MainItemStage.AddItemGroup(label);

            rb.OnGetGroupFor.Subscribe(0.01f, ResolveAlphabetGroup);
            bool ResolveAlphabetGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb) {
                gb = default;
                return false;
            }
        }

        private static void CloneSettings(RequestBuilder rb) {
            LocalSettings l = AlphabetRando.localSettings;
            GlobalSettings g = AlphabetRando.globalSettings;
            l.Vowels = g.Vowels;
            l.Consonants = g.Consonants;
            l.Numbers = g.Numbers;
            l.Custom = g.Custom && g.CustomCharacters.Length > 0;
            l.CustomCharacters = g.CustomCharacters;
        }
    }
}
