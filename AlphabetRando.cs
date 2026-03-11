using Modding;
using System.Collections.Generic;
using UnityEngine;

namespace AlphabetRando {
    public class AlphabetRando: Mod, IGlobalSettings<GlobalSettings>, ILocalSettings<LocalSettings> {
        new public string GetName() => "AlphabetRando";
        public override string GetVersion() => "1.1.0.0";

        public static GlobalSettings globalSettings { get; set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => globalSettings = s;
        public GlobalSettings OnSaveGlobal() => globalSettings;

        public static LocalSettings localSettings { get; set; } = new();
        public void OnLoadLocal(LocalSettings s) => localSettings = s;
        public LocalSettings OnSaveLocal() => localSettings;

        internal static AlphabetRando instance;

        public AlphabetRando(): base() {
            instance = this;
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            RandoInterop.Hook();
        }
    }
}