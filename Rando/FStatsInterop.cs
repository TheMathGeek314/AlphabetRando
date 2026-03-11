using System;
using System.Collections.Generic;
using System.Linq;
using FStats;
using FStats.StatControllers;
using FStats.Util;

namespace AlphabetRando {
    public static class FStatsInterop {
        public static void Hook() {
            API.OnGenerateFile += GenerateStats;
        }

        private static void GenerateStats(Action<StatController> generateStats) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            generateStats(new AbcStats());
        }

        public class AbcStats: StatController {
            public override void Initialize() {
                AbcModule.OnItemObtained += AddEntry;
            }

            public override void Unload() {
                AbcModule.OnItemObtained -= AddEntry;
            }

            public record KeyItem(string item, float timestamp);
            public List<KeyItem> KeyItems = [];
            public List<string> UsedKeys = [];

            public void AddEntry(string entry) {
                if(!UsedKeys.Contains(entry)) {
                    KeyItem key = new(entry, FStatsMod.LS.Get<Common>().CountedTime);
                    KeyItems.Add(key);
                    UsedKeys.Add(entry);
                }
            }

            public override IEnumerable<DisplayInfo> GetDisplayInfos() {
                List<string> rows = KeyItems.OrderBy(x => x.timestamp).Select(x => $"{x.item}: {x.timestamp.PlaytimeHHMMSS()}").ToList();
                yield return new() {
                    Title = "ABC",
                    MainStat = "",
                    StatColumns = Columnize(rows),
                    Priority = BuiltinScreenPriorityValues.ExtensionStats
                };
            }

            private const int COL_SIZE = 10;
            private List<string> Columnize(List<string> rows) {
                int columnCount = (rows.Count + COL_SIZE - 1) / COL_SIZE;
                List<string> list = [];
                for(int i = 0; i < columnCount; i++) {
                    list.Add(string.Join("\n", rows.Slice(i, columnCount)));
                }
                return list;
            }
        }
    }
}
