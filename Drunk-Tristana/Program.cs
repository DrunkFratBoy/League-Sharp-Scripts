using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Drunk_Tristana
{
    class Program
    {
        public static Menu Config;
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static Orbwalking.Orbwalker Orbwalker;
        public static SpellSlot igniteSlot;


        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName.ToLower() != "tristana") return;

            // Champion Spells
            Q = new Spell(SpellSlot.Q); // Rapid Fire
            W = new Spell(SpellSlot.W, 900); // Rocket Jump
            E = new Spell(SpellSlot.E, 541); // Add a check to add +9 range per level: Explosive Shot
            R = new Spell(SpellSlot.R, 541); // Add a check to add +9 range per level: Buster Shot
            igniteSlot = Player.GetSpellSlot("SummonerDot");

            // Config Menu
            Config = new Menu("Drunk-Tristana", "Tristana", true);
            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            // Orbwalking
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            // Combo menu
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("Use QC", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("Use EC", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("Use RC", "Use R").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "R if it Hits >=").SetValue(new Slider(2, 1, 5)));

            // Harass
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("Use EH", "Use E").SetValue(true));
            Config.SubMenu("Harass").AddItem(new MenuItem("Hmana", "Min. Mana Percent Harass").SetValue(new Slider(50, 100, 0)));

            // Auto Farm added eventually?

            // Misc Menu
            Config.AddSubMenu(new Menu("Misc", "Misc"));
            Config.SubMenu("Misc").AddItem(new MenuItem("UseRKs", "Auto R KS").SetValue(true));
            Config.SubMenu("Misc").AddItem(new MenuItem("UseEKs", "Auto E KS").SetValue(true));
            Config.SubMenu("KS").AddItem(new MenuItem("IgKs", "Use Ignite KS").SetValue(true));

            // Draw other Menu
            Config.AddSubMenu(new Menu("Draw", "DrawSettings"));
            Config.SubMenu("DrawSettings").AddItem(new MenuItem("Draw E", "E Range").SetValue(false));

            // Add menu
            Config.AddToMainMenu();
            Game.PrintChat("Drunk Tristana Provied by: DrunkFratBoy");
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;

            //Test sdfsf/// dfsafdsafsfsfsa

            throw new NotImplementedException(); // End of the function
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
