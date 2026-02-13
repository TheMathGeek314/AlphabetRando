using Modding;
using ItemChanger;
using RandomizerMod.RC;

namespace AlphabetRando {
    internal static class RandoInterop {
        public static void Hook() {
            RandoMenuPage.Hook();
            RequestModifier.Hook();
            LogicAdder.Hook();

            AbcModule.PrepareModule();
            RandoController.OnExportCompleted += AddAbcModule;

            DefineItems();

            if(ModHooks.GetMod("CondensedSpoilerLogger") is Mod)
                CondensedSpoilerLogger.AddCategory("Alphabet", (args) => true, Consts.itemNames);

            if(ModHooks.GetMod("RandoSettingsManager") is Mod)
                RSMInterop.Hook();
        }

        private static void AddAbcModule(RandoController controller) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            ItemChangerMod.Modules.GetOrAdd<AbcModule>();
        }

        public static void DefineItems() {
            foreach(string letter in Consts.allLetters) {
                LetterItem item = new(letter);
                Finder.DefineCustomItem(item);
            }
        }
    }
}
