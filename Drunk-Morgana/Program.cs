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
 
    /*
     Author: Ron Swanson
     Date: 2/23/2015
     Script Name: Drunk Morgana
     Enjoy (:
     
     */
    
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

            //Config Harrass
            Config.AddSubMenu(new Menu("Harass", "Harass"));
            Config.SubMenu("Harass").AddItem(new MenuItem("UseQHarass", "Use Q")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("UseWHarass", "Use W")).SetValue(true);
            Config.SubMenu("Harass").AddItem(new MenuItem("ActivateHarass", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press, false)));

            // Drwaing Range
            //Config.AddSubMenu(new Menu("Drawings", "Drawings"));
            //Config.SubMenu("Drawings").AddItem(new MenuItem("DrawEnable", "Enable Drawing"));

            Config.AddToMainMenu();
            Game.OnGameUpdate += Game_OnGameUpdate;
            

            
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            if (Config.Item("ActivateCombo").GetValue<KeyBind>().Active)
            {
                Combo();
            }

            if (Config.Item("ActivateHarass").GetValue<KeyBind>().Active)
            {
                Harass();
            }
        }

        static void ProtectAlly()
        {
            if (Player.IsAlly && Player.MagicDamageTaken >= 100)
            {
                E.CastOnUnit(;
            }
        }

        private static void Harass()
        {

            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
            if (target == null) return;

            //Harass
            var prediction = Q.GetPrediction(target); // Create prediction based on Q value
            if (prediction.Hitchance >= HitChance.High && CollisionableObjects.Minions == 0 || CollisionableObjects.YasuoWall == 0) 
            {
                Q.Cast();
            }

            if (prediction.Hitchance >= HitChance.High)
            {
                W.Cast(target);
            }



        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(Q.Range,TargetSelector.DamageType.Magical); // Get the target
            if (target == null) return;

            //Combo
            var prediction = Q.GetPrediction(target);// Create predition based on Q values
            if (prediction.Hitchance >= HitChance.High && CollisionableObjects.Minions == 0)
            {
                Q.Cast();
            }
            
            /* if (prediction.Hitchance >= HitChance.High && prediction.CollisionObjects.Count(h => h is Obj_AI_Minion) == 0 ) {
                Q.Cast(prediction.CastPosition); // Cast Q on the predicted target

                // R Combo - position.CountEnemysInRange(range) - (use CountEnemiesInRange I just obsoleted CountEnemysInRange)      
            }
             * */

            if (prediction.Hitchance >= HitChance.High)
            {
                W.Cast(target);
            }

            if (prediction.AoeTargetsHitCount >= 2)
            {
                R.Cast();
            }
           

        }
    }
}
