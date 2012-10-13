using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace D3Bit
{
    public static class Data
    {

        public static List<string> ItemQualities = new List<string>
                                                   {
                                                       "Magic",
                                                       "Rare",
                                                       "Legendary",
                                                       "Set"
                                                   };

        public static List<string> WeaponTypes = new List<string>
                                                   {
                                                       "Axe",
                                                       "Ceremonial Knife",
                                                       "Hand Crossbow",
                                                       "Dagger",
                                                       "Fist Weapon",
                                                       "Mace",
                                                       "Mighty Weapon",
                                                       "Spear",
                                                       "Sword",
                                                       "Wand",
                                                       "Two-Handed Axe",
                                                       "Bow",
                                                       "Daibo",
                                                       "Crossbow",
                                                       "Two-Handed Mace",
                                                       "Two-Handed Mighty Weapon",
                                                       "Polearm",
                                                       "Staff",
                                                       "Two-Handed Sword"
                                                   };
        public static List<string> OffHandTypes = new List<string>
                                                   {
                                                       "Mojo",
                                                       "Source",
                                                       "Quiver"
                                                   };
        public static List<string> FollowerTypes = new List<string>
                                                   {
                                                       "Enchantress Focus",
                                                       "Scoundrel Token",
                                                       "Templar Relic"
                                                   };
        public static List<string> CommonTypes = new List<string>
                                                   {
                                                       "Shield",
                                                       "Ring",
                                                       "Amulet",
                                                       "Shoulders",
                                                       "Helm",
                                                       "Pants",
                                                       "Gloves",
                                                       "Chest Armor",
                                                       "Bracers",
                                                       "Boots",
                                                       "Belt",
                                                       "Cloak",
                                                       "Mighty Belt",
                                                       "Spirit Stone",
                                                       "Voodoo Mask",
                                                       "Wizard Hat"
                                                   };

        public static List<string> ItemTypes = WeaponTypes.Union(OffHandTypes).Union(CommonTypes).ToList();

        public static Dictionary<string, string> affixMatches = new Dictionary<string, string>
                                                             {
                                                                 //Main Stats
                                                                 {"Str", "+{I} Strength"},
                                                                 {"Int", "+{I} Intelligence"},
                                                                 {"Dex", "+{I} Dexterity"},
                                                                 {"Vit", "+{I} Vitality"},
                                                                 //Resist
                                                                 {"AR", "+{I} Resistance to All Elements"},
                                                                 {"ArcR", "+{I} Arcane Resistance"},
                                                                 {"ColdR", "+{I} Cold Resistance"},
                                                                 {"FireR", "+{I} Fire Resistance"},
                                                                 {"PoisonR", "+{I} Poison Resistance"},
                                                                 {"LtnR", "+{I} Lightning Resistance"},
                                                                 {"PhyR", "+{I} Physical Resistance"},
                                                                 //Damage-Related
                                                                 {"ArcD", "+{I}-{I} Arcane Damage"},
                                                                 {"ColdD", "+{I}-{I} Cold Damage"},
                                                                 {"FireD", "+{I}-{I} Fire Damage"},
                                                                 {"HolyD", "+{I}-{I} Holy Damage"},
                                                                 {"PoisonD", "+{I}-{I} Poison Damage"},
                                                                 {"LtnD", "+{I}-{I} Lightning Damage"},
                                                                 {"Dmg", "+- Damage"},
                                                                 {"Dmg%", "+% Damage"},
                                                                 {"MinD", "+{I} Minimum Damage"},
                                                                 {"MaxD", "+{I} Maximum Damage"},
                                                                 {"AtkSpd", "Increases Attack Speed by {I}%"},
                                                                 {"Crit", "Critical Hit Chance Increased by {D}%"},
                                                                 {"CritD", "Critical Hit Damage Increased by {I}%"},
                                                                 //Life-Related
                                                                 {"LoH", "Each Hit Adds +{I} Life"},
                                                                 {"LoK", "+{I} Life after Each Kill"},
                                                                 {"LS", "{D}% of Damage Dealt Is Converted to Life"},
                                                                 {"LPS", "Gain {D} per Spirit Spent"},
                                                                 {"Life%", "+% Life"},
                                                                 {"LRegen", "Regenerates {I} Life per Second"},
                                                                 {"GlobeHP", "Health Globes grant +{I} Life."},
                                                                 //Adventure Stats
                                                                 {"MF", "{I}% Better Chance of Finding Magical Items"},
                                                                 {"GF", "{I}% Extra Gold from Monsters"},
                                                                 {"Exp", "Monster kills grant +{I} experience."},
                                                                 {"Exp%", "Increases Bonus Experience by {I}%"},
                                                                 {"MvSpd", "+{I}% Movement Speed"},
                                                                 {"Pick", "Increases Gold and Health Pickup by {I} Yards"},
                                                                 {"LvlRe", "Level Requirement Reduced by {I}"},
                                                                 //Resource
                                                                 {"MaxArcP", "+{I} Maximum Arcane Power"},
                                                                 {"ArcPCrit", "Critical Hits grant {I} Arcane Power"},
                                                                 {"HateRegen", "Increases Hatred Regeneration by {D} per Second"},
                                                                 {"MaxMana", "+{I} Maximum Mana"},
                                                                 {"ManaRegen", "Increases Mana Regeneration by {D} per Second"},
                                                                 {"MaxDisc", "+{I} Maximum Discipline"},
                                                                 {"MaxFury", "+{I} Maximum Fury"},
                                                                 {"SpiritRegen", "Increases Spirit Regeneration by {D} per Second"},
                                                                 //CC
                                                                 {"Chill", "{D}% Chance to Chill on Hit"},
                                                                 {"Fear", "{D}% Chance to Fear on Hit"},
                                                                 {"Freeze", "{D}% Chance to Freeze on Hit"},
                                                                 {"Immobilize", "{D}% Chance to Immobilize on Hit"},
                                                                 {"Knockback", "{D}% Chance to Knockback on Hit"},
                                                                 {"Slow", "{D}% Chance to Slow on Hit"},
                                                                 {"Stun", "{D}% Chance to Stun on Hit"},
                                                                 {"ReCC", "Reduces duration of control impairing effects by {I}%"},
                                                                 //Damage Reduction
                                                                 {"RDElite", "Reduces damage from elites by {I}%."},
                                                                 {"RDMelee", "Reduces damage from melee attacks by {I}%."},
                                                                 {"RDRanged", "Reduces damage from ranged attacks by {I}%."},
                                                                 //Damage Increase
                                                                 {"IDElite", "Increase Damage Against Elites by {I}%."},
                                                                 //Misc
                                                                 {"Armor", "+{I} Armor"},
                                                                 {"Block", "+{I}% Chance to Block"},
                                                                 {"Ind", "Ignores Durability Loss"},
                                                                 {"Thorn", "Melee attackers take {I} damage per hit"},
                                                                 {"Bleed", "{D}% chance to inflict Bleed for {I}-{I} damage over 5 seconds."},
                                                                 {"Soc", "Empty Socket"},
                                                                 //Non-Armor Skills
                                                                 {"B-HoA", "Reduces resource cost of Hammer of the Ancients by {i} Fury."},
                                                                 {"B-OP", "Increases Critical Chance of Overpower by {i}%"},
                                                                 {"B-SS", "Increases Critical Chance of Seismic Slam by {i}%"},
                                                                 {"B-WW", "Increases Critical Chance of Whirlwind by {i}%"},
                                                                 {"D-BS", "Increases Bola Shot Damage by {i}%"},
                                                                 {"D-EA", "Increases Elemental Arrow Damage by {i}%"},
                                                                 {"D-ES", "Increases Entangling Shot Damage by {i}%"},
                                                                 {"D-HA", "Increases Hungering Arrow Damage by {i}%"},
                                                                 {"D-MS", "Increases Critical Chance of Multishot by {i}%"},
                                                                 {"D-RF", "Increases Critical Chance of Rapid Fire by {i}%"},
                                                                 {"M-LTK", "Reduces resource cost of Lashing Tail Kick by {i} Spirit."},
                                                                 {"M-TR", "Increases Critical Chance of Tempest Rush by {i}%"},
                                                                 {"M-WoL", "Increases Critical Chance of Wave of Light by {i}%"},
                                                                 {"W-FBomb", "Reduces resource cost of Firebomb by {i} Mana."},
                                                                 {"W-Haunt", "Increases Haunt Damage by {i}%"},
                                                                 {"W-PoT", "Increases Plague of Toads Damage by {i}%"},
                                                                 {"W-PD", "Increases Poison Dart Damage by {i}%"},
                                                                 {"W-SB", "Increases Spirit Barrage Damage by {i}%"},
                                                                 {"W-WoZ", "Reduces cooldown of Wall of Zombies by {i} seconds."},
                                                                 {"W-ZC", "Reduces resource cost of Zombie Charger by {i} Mana."},
                                                                 {"Z-ET", "Increases Critical Chance of Energy Twister by {i}%"},
                                                                 {"Z-MM", "Increases Magic Missile Damage by {i}%"},
                                                                 {"Z-AO", "Increases Critical Chance of Arcane Orb by {i}%"},
                                                                 {"Z-Blizz", "Increases duration of Blizzard by {i} seconds"},
                                                                 {"Z-Meteor", "Reduces resource cost of Meteor by {i} Arcane Power."},
                                                                 {"Z-SP", "Increases Shock Pulse Damage by {i}%"},
                                                                 {"Z-SB", "Increases Spectral Blade Damage by {i}%"},
                                                                 //Armor Skills
                                                                 {"B-Bash", "Increases Bash Damage by {i}%"},
                                                                 {"B-Cleave", "Increases Cleave Damage by {i}%"},
                                                                 {"B-Frenzy", "Increases Frenzy Damage by {i}%"},
                                                                 {"B-Rend", "Reduces resource cost of Rend by {i} Fury."},
                                                                 {"B-Revenge", "Increases Critical Chance of Revenge by {i}%"},
                                                                 {"B-WThrow", "Reduces resource cost of Weapon Throw by {i} Fury."},
                                                                 {"D-Chakram", "Reduces resource cost of Chakram by {i} Hatred."},
                                                                 {"D-EFire", "Increases Evasive Fire Damage by {i}%"},
                                                                 {"D-Grenades", "Increases Grenades Damage by {i}%"},
                                                                 {"D-Impale", "Reduces resource cost of Impale by {i} Hatred."},
                                                                 {"D-STrap", "Increases Spike Trap Damage by {i}%"},
                                                                 {"M-CWave", "Increases Crippling Wave Damage by {i}%"},
                                                                 {"M-CStrke", "Reduces resource cost of Cyclone Strike by {i} Spirit."},
                                                                 {"M-DReach", "Increases Deadly Reach Damage by {i}%"},
                                                                 {"M-EPalm", "Increases Exploding Palm Damage by {i}%"},
                                                                 {"M-FoT", "Increases Fists of Thunder Damage by {i}%"},
                                                                 {"M-SWind", "Increases Sweeping Wind Damage by {i}%"},
                                                                 {"M-WotHF", "Increases Way of the Hundred Fists Damage by {i}%"},
                                                                 {"W-ACloud", "Increases Critical Chance of Acid Cloud by {i}%"},
                                                                 {"W-FBats", "Reduces resource cost of Fire Bats by {i} Mana."},
                                                                 {"W-LSwarm", "Increases Locust Swarm Damage by {i}%"},
                                                                 {"W-ZDogs", "Reduces cooldown of Summon Zombie Dogs by {i} seconds."},
                                                                 {"Z-ATorrent", "Reduces resource cost of Arcane Torrent by {i} Arcane Power."},
                                                                 {"Z-DisInt", "Reduces resource cost of Disintegrate by {i} Arcane Power."},
                                                                 {"Z-Elec", "Increases Electrocute Damage by {i}%"},
                                                                 {"Z-EB", "Increases Critical Chance of Explosive Blast by {i}%"},
                                                                 {"Z-Hydra", "Reduces resource cost of Hydra by {i} Arcane Power."},
                                                                 {"Z-RoF", "Increases Critical Chance of Ray of Frost by {i}%"}
                                                             };

        public static void LoadAffixes(string languageCode)
        {
            string json = File.ReadAllText(string.Format(@"data\affixes.{0}.json", languageCode));
            affixMatches = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

    }
}
