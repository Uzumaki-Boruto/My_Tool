using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using EloBuddy.SDK.Events;

namespace Database_Collection
{
    class Program
    {
        static void Main(string[] args)
        {
            Read_Database();
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            SpellSlot[] spelllist = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R, SpellSlot.Summoner1, SpellSlot.Summoner2 };
            foreach (var spell in spelllist)
            {
                var sp = new Spell.Skillshot(spell);
                if (!sp.Range.Equals(0) && !SpellData.Any(x => x.DisplayName.Equals(sp.Name)))
                {
                    SpellData.Add(new Missile_Information(sp));
                }
            }
            Write_Database();
            GameObject.OnCreate += GameObject_OnCreate;
            Game.OnEnd += Game_OnEnd;
            Game.OnDisconnect += Game_OnDisconnect;
            Game.OnNotify += Game_OnNotify;
            Game.OnUpdate += Game_OnUpdate;
        }
        private const string FileName = "Database.json";
        private const string FileName2 = "Real Spells.json";
        private static string MissilePath = EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\Missile Database\";
        //private static List<Tuple<MissileClient, bool>> Data = new List<Tuple<MissileClient, bool>>();
        private static List<Missile_Information> SpellData = new List<Missile_Information>();
        private static List<Missile_Information> MissileData = new List<Missile_Information>();
        private static bool Writen { get; set; }
        private static bool Seen { get; set; }
        private static float LastWrite { get; set; }
        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            var missile = sender as MissileClient;
            if (missile == null || !(missile.SpellCaster is AIHeroClient)) return;
            var caster = missile.SpellCaster as AIHeroClient;
            if (!MissileData.Any(x => x.Name.Equals(missile.SData.Name)))
            {
                //Add a value
                MissileData.Add(new Missile_Information(missile));
                Console.WriteLine("Added " + caster.ChampionName + " - " + missile.SData.Name);
            }
            else
            {
                //Replace a value
                MissileData[MissileData.FindIndex(x => x.Name.Equals(missile.SData.Name) && x.Slot.Equals(missile.Slot))] = new Missile_Information(missile);
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (LastWrite + 60 < Game.Time)
            {
                Write_Database();
                Read_Database();
            }
        }
        private static void Game_OnEnd(GameEndEventArgs args)
        {
            if (!Writen)
            {
                Write_Database();
            }
        }
        private static void Game_OnDisconnect(EventArgs args)
        {
            if (!Writen)
            {
                Write_Database();
            }
        }
        private static void Game_OnNotify(GameNotifyEventArgs args)
        {
            if (args.EventId.Equals(GameEventId.OnQuit) && ObjectManager.GetUnitByNetworkId<AIHeroClient>(args.NetworkId) != null
                && ObjectManager.GetUnitByNetworkId<AIHeroClient>(args.NetworkId).IsMe && !Writen)
            {
                Write_Database();
            }
        }
        private static bool Write_Database()
        {
            LastWrite = Game.Time;
            if (MissileData.Any() || SpellData.Any())
            {
                if (!Directory.Exists(MissilePath))
                {
                    Directory.CreateDirectory(MissilePath);
                }
                if (SpellData.Any())
                {
                    string data = JsonConvert.SerializeObject(SpellData.OrderBy(x => x.SpellCaster), Formatting.Indented, new StringEnumConverter() { AllowIntegerValues = true });
                    File.WriteAllText(Path.Combine(MissilePath, FileName2), data);
                }
                //var ordered = MissileData.OrderBy(x => new { x.SpellCaster, x.Slot, });
                string json = JsonConvert.SerializeObject(MissileData.OrderBy(x => x.SpellCaster), Formatting.Indented, new StringEnumConverter() { AllowIntegerValues = true });
                File.WriteAllText(Path.Combine(MissilePath, FileName), json);
                Writen = true;
                Seen = false;
                return true;
            }
            else
            {
                Writen = false;
                return false;
            }
        }
        private static bool Read_Database()
        {
            if (!Directory.Exists(MissilePath))
            {
                Seen = false;
                return false;
            }
            else
            {
                if (!File.Exists(Path.Combine(MissilePath, FileName)) && !File.Exists(Path.Combine(MissilePath, FileName2)))
                {
                    Seen = false;
                    return false;
                }
                else
                {
                    if (File.Exists(Path.Combine(MissilePath, FileName)))
                    {
                        string read = File.ReadAllText(Path.Combine(MissilePath, FileName));
                        MissileData = JsonConvert.DeserializeObject<List<Missile_Information>>(read, new JsonSerializerSettings() { Formatting = Formatting.Indented, });
                    }
                    if (File.Exists(Path.Combine(MissilePath, FileName2)))
                    {
                        string read2 = File.ReadAllText(Path.Combine(MissilePath, FileName2));
                        SpellData = JsonConvert.DeserializeObject<List<Missile_Information>>(read2, new JsonSerializerSettings() { Formatting = Formatting.Indented, });
                    }
                    Seen = true;
                    Writen = false;
                    return true;
                }
            }
        }
    }
}
