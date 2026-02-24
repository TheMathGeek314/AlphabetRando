using System;
using System.Reflection;
using System.Text;
using MonoMod.RuntimeDetour;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlphabetRando {
    internal class AbcModule: ItemChanger.Modules.Module {
        private static Hook tHook, tmpHook;
        private static NativeDetour tmDetour;
        private static Action<TextMesh, string> origTM;
        private static MethodInfo tSetText, tmSetText, tmpSetText;

        public static void PrepareModule() {
            BindingFlags f = BindingFlags.Public | BindingFlags.Instance;
            string setText = "set_text";
            tSetText = typeof(Text).GetMethod(setText, f);
            tmSetText = typeof(TextMesh).GetMethod(setText, f);
            tmpSetText = typeof(TMP_Text).GetMethod(setText, f);
        }

        public override void Initialize() {
            tHook = new(tSetText, ReplaceText);
            tmDetour = new(tmSetText, typeof(AbcModule).GetMethod("ReplaceTextMesh", BindingFlags.NonPublic | BindingFlags.Static));
            tmpHook = new(tmpSetText, ReplaceTextMeshPro);
            origTM = tmDetour.GenerateTrampoline<Action<TextMesh, string>>();
        }

        public override void Unload() {
            tHook.Dispose();
            tHook = null;
            tmDetour.Dispose();
            tmDetour = null;
            tmpHook.Dispose();
            tmpHook = null;
            origTM = null;
        }

        private static void ReplaceText(Action<Text, string> orig, Text self, string origText) {
            orig(self, FilterOnTerms(origText));
        }

        private static void ReplaceTextMesh(TextMesh self, string origText) {
            origTM(self, FilterOnTerms(origText));
        }

        private static void ReplaceTextMeshPro(Action<TextMeshPro, string> orig, TextMeshPro self, string origText) {
            orig(self, FilterOnTerms(origText));
        }

        private static string FilterOnTerms(string text) {
            if(text == null)
                return null;
            (bool, string[])[] tuples = [
                (AlphabetRando.localSettings.Vowels, Consts.vowels),
                (AlphabetRando.localSettings.Consonants, Consts.consonants)
            ];
            foreach((bool setting, string[] letters) in tuples) {
                if(!setting)
                    continue;
                foreach(string letter in letters) {
                    if(RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get("Alphabet-" + letter) == 0) {
                        text = RemoveLettersOutsideTags(text, letter[0]).Replace("Alphabet-" + letter, letter).Replace("Alphabet " + letter, letter);
                    }
                    else {
                        text = text.Replace("Alphabet-" + letter, letter);
                    }
                }
            }
            return text;
        }

        private static string RemoveLettersOutsideTags(string input, char letter) {
            while(true) {
                int abcIndex = input.IndexOf("Alphabet");
                if(abcIndex < 0)
                    break;
                string before = abcIndex < 1 ? "" : RemoveLettersOutsideTags(input.Substring(0, abcIndex), letter);
                string after = input.Length <= abcIndex + 10 ? "" : RemoveLettersOutsideTags(input.Substring(abcIndex + 10), letter);
                return before + input.Substring(abcIndex, 10) + after;
            }

            StringBuilder sb = new(input.Length);
            bool insideTag = false;
            char lower = char.ToLowerInvariant(letter);
            foreach(char c in input) {
                if(c == '<') {
                    insideTag = true;
                    sb.Append(c);
                    continue;
                }
                if(c == '>') {
                    insideTag = false;
                    sb.Append(c);
                    continue;
                }
                if(!insideTag && (c == lower || c == letter))
                    continue;
                sb.Append(c);
            }
            string output = sb.ToString();
            return output.Length == 0 ? " " : output;
        }
    }
}
