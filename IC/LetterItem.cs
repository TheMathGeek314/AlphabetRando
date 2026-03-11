using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AlphabetRando {
    internal class LetterItem: AbstractItem {
        public LetterItem(string letter) {
            name = "Alphabet-" + letter;
            InteropTag tag = GetOrAddTag<InteropTag>();
            tag.Message = "RandoSupplementalMetadata";
            tag.Properties["ModSource"] = AlphabetRando.instance.GetName();
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_letter");
            UIDef = new BigUIDef() {
                bigSprite = new EmbeddedSprite("big_abcblocks"),
                name = new BoxedString(name),
                shopDesc = new BoxedString("The quick brown fox jumps over the lazy dog."),
                sprite = new EmbeddedSprite("pin_letter")
            };
        }

        public override bool Redundant() {
            return RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get(name) > 0;
        }

        public override void GiveImmediate(GiveInfo info) {
            AbcModule.Instance.RecordItem(name);
        }
    }
}
