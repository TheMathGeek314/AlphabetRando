using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace AlphabetRando {
    public class RandoMenuPage {
        internal MenuPage AbcRandoPage;
        internal MenuElementFactory<GlobalSettings> abcMEF;
        internal VerticalItemPanel abcVIP;

        internal SmallButton JumpToAbcButton;

        internal static RandoMenuPage Instance { get; private set; }

        public static void OnExitMenu() {
            Instance = null;
        }

        public static void Hook() {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button) {
            button = Instance.JumpToAbcButton;
            return true;
        }

        private void SetTopLevelButtonColor() {
            if(JumpToAbcButton != null) {
                JumpToAbcButton.Text.color = AlphabetRando.globalSettings.Any ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private static void ConstructMenu(MenuPage landingPage) => Instance = new(landingPage);

        private RandoMenuPage(MenuPage landingPage) {
            AbcRandoPage = new MenuPage(Localize("AlphabetRando"), landingPage);
            abcMEF = new(AbcRandoPage, AlphabetRando.globalSettings);
            abcVIP = new(AbcRandoPage, new(0, 300), 75f, true, abcMEF.Elements);
            Localize(abcMEF);
            foreach(IValueElement e in abcMEF.Elements) {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            JumpToAbcButton = new(landingPage, Localize("AlphabetRando"));
            JumpToAbcButton.AddHideAndShowEvent(landingPage, AbcRandoPage);
            SetTopLevelButtonColor();
        }
    }
}
