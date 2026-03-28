using Modding;
using System.IO;
using System.Reflection;
using Satchel.BetterMenus;
using System.Linq;

namespace AlphabetRando {
    public static class CustomCharacterHandler {
        private static Menu MenuRef;
        private static string defaultChars = ".,?!'\"-_()[]:+";

        public static void LoadFile(bool manualReload) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string filename = Path.GetDirectoryName(assembly.Location) + "/CustomCharacters.txt";
            if(!File.Exists(filename)) {
                using FileStream fs = File.Create(filename);
                using(StreamWriter writer = new(fs)) {
                    writer.Write(defaultChars);
                };
            }
            string content = File.ReadAllText(filename);
            string previous = AlphabetRando.globalSettings.CustomCharacters;
            AlphabetRando.globalSettings.CustomCharacters = "";
            foreach(char c in content) {
                if(char.IsWhiteSpace(c))
                    continue;
                if(c == '<' || c == '>')
                    continue;
                if(Consts.allLetters.Contains(c.ToString().ToUpper()))
                    continue;
                if(AlphabetRando.globalSettings.CustomCharacters.Contains(c))
                    continue;
                AlphabetRando.globalSettings.CustomCharacters += c;
            }
            if(manualReload)
                RandoInterop.DefineCustomCharacterItems(previous);
        }

        public static MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            MenuRef ??= new Menu(
                name: "Alphabet Rando",
                elements: new Element[] {
                    new MenuButton(
                        name: "Reload Custom Characters",
                        description: "",
                        submitAction: (Mbutton) => {
                            LoadFile(true);
                            MenuRef.Find("AbcReloadDisplay").Name = AlphabetRando.globalSettings.CustomCharacters;
                            MenuRef.Update();
                        },
                        Id: "AbcReloadBtn"
                    ),
                    new TextPanel(""),
                    new TextPanel("Current Characters:"),
                    new TextPanel(AlphabetRando.globalSettings.CustomCharacters, Id: "AbcReloadDisplay")
                }
            );
            return MenuRef.GetMenuScreen(modListMenu);
        }
    }
}
