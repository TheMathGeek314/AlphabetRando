using Modding;
using System.IO;
using ItemChanger;
using RandomizerMod.Logging;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace AlphabetRando {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            AbcModule.PrepareModule();
            RandoController.OnExportCompleted += AddAbcModule;
            SettingsLog.AfterLogSettings += LogRandoSettings;

            DefineItems();

            if(ModHooks.GetMod("CondensedSpoilerLogger") is Mod)
                CondensedSpoilerLogger.AddCategory("Alphabet", (args) => true, Consts.itemNames);

            if(ModHooks.GetMod("FStatsMod") is Mod)
                FStatsInterop.Hook();

            if(ModHooks.GetMod("RandoSettingsManager") is Mod)
                RSMInterop.Hook();
        }

        private static void AddAbcModule(RandoController controller) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            ItemChangerMod.Modules.GetOrAdd<AbcModule>();
        }

        private static void LogRandoSettings(LogArguments args, TextWriter w) {
            w.WriteLine("Logging AlphabetRando settings:");
            w.WriteLine(JsonUtil.Serialize(AlphabetRando.globalSettings));
        }

        public static void DefineItems() {
            foreach(string letter in Consts.allLetters) {
                LetterItem item = new(letter);
                Finder.DefineCustomItem(item);
            }
            DefineCustomCharacterItems("");
        }

        public static void DefineCustomCharacterItems(string previousChars) {
            foreach(char prev in previousChars) {
                Finder.UndefineCustomItem("Alphabet-" + prev);
            }
            foreach(char letter in AlphabetRando.globalSettings.CustomCharacters) {
                LetterItem item = new(letter.ToString());
                Finder.DefineCustomItem(item);
            }
        }
    }
}
