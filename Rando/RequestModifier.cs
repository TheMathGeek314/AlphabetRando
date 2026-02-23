using System.Linq;
using ItemChanger;
using RandomizerCore.Exceptions;
using RandomizerCore.Randomization;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace AlphabetRando {
    public class RequestModifier {
        public static void Hook() {
            RequestBuilder.OnUpdate.Subscribe(-499, SetupItems);
            RequestBuilder.OnUpdate.Subscribe(-499.5f, DefinePools);
            RequestBuilder.OnUpdate.Subscribe(101, RestrictPlacements);//hopefully temporary
            RequestBuilder.OnUpdate.Subscribe(0, CloneSettings);
        }

        private static void SetupItems(RequestBuilder rb) {
            if(!AlphabetRando.globalSettings.Any)
                return;

            GlobalSettings gs = AlphabetRando.globalSettings;
            foreach(string letter in Consts.itemNames) {
                rb.EditItemRequest(letter, info => {
                    info.getItemDef = () => new ItemDef() {
                        Name = letter,
                        Pool = "Alphabet",
                        MajorItem = true,
                        PriceCap = 26
                    };
                });
            }
            if(gs.Vowels) {
                foreach(string vowel in Consts.vowels) {
                    rb.AddItemByName("Alphabet-" + vowel, gs.DupeVowels ? 2 : 1);
                }
            }
            if(gs.Consonants) {
                foreach(string consonant in Consts.consonants) {
                    rb.AddItemByName("Alphabet-" + consonant, gs.DupeConsonants ? 2 : 1);
                }
            }
        }

        private static void DefinePools(RequestBuilder rb) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            ItemGroupBuilder group = null;
            string label = RBConsts.SplitGroupPrefix + "Alphabet";
            foreach(ItemGroupBuilder igb in rb.EnumerateItemGroups()) {
                if(igb.label == label) {
                    group = igb;
                    break;
                }
            }
            group ??= rb.MainItemStage.AddItemGroup(label);

            rb.OnGetGroupFor.Subscribe(0.01f, ResolveAlphabetGroup);
            bool ResolveAlphabetGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb) {
                gb = default;
                return false;
            }
        }

        //hotfix, hopefully temporary
        private static void RestrictPlacements(RequestBuilder rb) {
            if(!AlphabetRando.globalSettings.Any)
                return;
            string[] tollLocations = new string[] { LocationNames.Stag_Nest_Stag, LocationNames.City_Storerooms_Stag, LocationNames.Crossroads_Stag, LocationNames.Distant_Village_Stag,
            LocationNames.Greenpath_Stag, LocationNames.Hidden_Station_Stag, LocationNames.Kings_Station_Stag, LocationNames.Queens_Gardens_Stag, LocationNames.Queens_Station_Stag,
            LocationNames.Ancient_Basin_Map, LocationNames.City_of_Tears_Map, LocationNames.Crossroads_Map, LocationNames.Crystal_Peak_Map, LocationNames.Deepnest_Map_Right,
            LocationNames.Deepnest_Map_Upper, LocationNames.Fog_Canyon_Map, LocationNames.Fungal_Wastes_Map, LocationNames.Greenpath_Map, LocationNames.Howling_Cliffs_Map,
            LocationNames.Kingdoms_Edge_Map, LocationNames.Queens_Gardens_Map, LocationNames.Resting_Grounds_Map, LocationNames.Royal_Waterways_Map, LocationNames.Elevator_Pass };
            foreach(ItemGroupBuilder igb in rb.EnumerateItemGroups()) {
                if(igb.strategy is DefaultGroupPlacementStrategy dgps) {
                    dgps.ConstraintList.Add(new DefaultGroupPlacementStrategy.Constraint(
                        (item, location) => !(item.Name.StartsWith("Alphabet-") && tollLocations.Contains(location.Name)),
                        Label: "Abc Placement",
                        Fail: (item, location) => throw new OutOfLocationsException()
                    ));
                }
            }
        }

        private static void CloneSettings(RequestBuilder rb) {
            LocalSettings l = AlphabetRando.localSettings;
            GlobalSettings g = AlphabetRando.globalSettings;
            l.Vowels = g.Vowels;
            l.Consonants = g.Consonants;
        }
    }
}
