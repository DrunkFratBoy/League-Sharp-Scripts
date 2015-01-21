using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Drunk_Morgana
{
    internal class Program
    {
        private const string Champion = "Morgana";
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Config;
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static List<Spell> SpellList = new List<Spell>();
        
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.BaseSkinName != Champion) return;

            // Define Spells
            Q = new Spell(SpellSlot.Q, 1175); // Dark Binding
            W = new Spell(SpellSlot.W, 1075); // Tormented Soil
            E = new Spell(SpellSlot.E, 750); // Black Shield
            R = new Spell(SpellSlot.R, 600); // Soul Shackles


            //Set SkillShot
            Q.SetSkillshot(1175.0f, 70, 1200, false, SkillshotType.SkillshotLine); // Dark Binding
            W.SetSkillshot(1075.0f, 350, 0, false, SkillshotType.SkillshotCircle); // Tormented Soil

            // Add spells to the SpellList, nothing much to explain here.
            SpellList.Add(Q); // Dark Binding
            SpellList.Add(W); // Tormented Soil
            SpellList.Add(E); // Black Shield
            SpellList.Add(R); // Soul Shackles

            // Create the menu
            Config = new Menu("Drunk-Morgana", "Drunk-Morgana", true);

            // Initiate Target Selector
            var TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            // Orbwalking
            Config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo Menu
            Config.AddSubMenu(new Menu("Combo", "Combo"));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQCombo", "Use Q")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseWCombo", "Use W")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseECombo", "Use E")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRCombo", "Use R")).SetValue(true);
            Config.SubMenu("Combo").AddItem(new MenuItem("ActivateCombo", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));

            // Drwaing Range
            //Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            //Config.SubMenu("Drawings").AddItem(new MenuItem("DrawEnable", "Enable Drawing"));

            Config.AddToMainMenu();
            Game.OnGameUpdate += Game_OnGameUpdate;
            Game.PrintChat("Welcome to Drunk-Morgana: Provied by DrunkFratBoy");

            
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ActivateCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range,TargetSelector.DamageType.Magical); // Get the target
            if (target == null) return;

            //Combo
            var prediction = Q.GetPrediction(target);// Create predition based on Q values
            Game.PrintChat("Before Combo");
            if (prediction.Hitchance >= HitChance.Low && prediction.CollisionObjects.Count(h => h.IsEnemy && !h.IsDead && h is Obj_AI_Minion) < 1) {
                Q.Cast(prediction.CastPosition); // Cast Q on the predicted target
                Game.PrintChat("During Combo");
            }
        }
    }
}
