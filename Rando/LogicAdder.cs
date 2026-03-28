using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;

namespace AlphabetRando {
    public static class LogicAdder {
        public static void Hook() {
            RCData.RuntimeLogicOverride.Subscribe(50, ApplyLogic);
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            foreach(string name in Consts.itemNames) {
                Term term = lmb.GetOrAddTerm(name, TermType.Int);
                lmb.AddItem(new SingleItem(name, new TermValue(term, 1)));
            }
            for(int i = 0; i < AlphabetRando.globalSettings.CustomCharacters.Length; i++) {
                Term term = lmb.GetOrAddTerm("Alphabet-Custom_" + i, TermType.Int);
                lmb.AddItem(new SingleItem("Alphabet-" + AlphabetRando.globalSettings.CustomCharacters[i], new TermValue(term, 1)));
            }
        }
    }
}
