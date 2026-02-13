using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace AlphabetRando {
    internal static class RSMInterop {
        public static void Hook() {
            RandoSettingsManagerMod.Instance.RegisterConnection(new AbcSettingsProxy());
        }
    }

    internal class AbcSettingsProxy: RandoSettingsProxy<GlobalSettings, string> {
        public override string ModKey => AlphabetRando.instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; } = new EqualityVersioningPolicy<string>(AlphabetRando.instance.GetVersion());

        public override void ReceiveSettings(GlobalSettings settings) {
            settings ??= new();
            RandoMenuPage.Instance.abcMEF.SetMenuValues(settings);
        }

        public override bool TryProvideSettings(out GlobalSettings settings) {
            settings = AlphabetRando.globalSettings;
            return settings.Any;
        }
    }
}