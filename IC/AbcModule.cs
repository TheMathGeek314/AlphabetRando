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
            tmpSetText = typeof(TextMeshPro).GetMethod(setText, f);
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
                        text = RemoveLettersOutsideTags(text, letter[0]);
                    }
                    else if(text.StartsWith("Alphabet") && text.Length == 10) {
                        text = text[9].ToString();
                    }
                }
            }
            return text;
        }

        private static string RemoveLettersOutsideTags(string input, char letter) {
            if(input.StartsWith("Alphabet")) {
                string prefix = letter == input[9] ? letter.ToString() : input.Substring(0, 10);
                return prefix + (input.Length == 10 ? "" : RemoveLettersOutsideTags(input.Substring(10), letter));
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
            return sb.ToString();
        }
    }
}
