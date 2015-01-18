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
            Config.SubMenu("Combo").AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            Config.SubMenu("Combo").AddItem(new MenuItem("UseRC", "Use R").SetValue(true));
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
            Config.SubMenu("Misc").AddItem(new MenuItem("UseAntiGapcloser", "R on Gapclose").SetValue(true));
            Config.SubMenu("KS").AddItem(new MenuItem("IgKs", "Use Ignite KS").SetValue(true));

            // Draw other Menu
            Config.AddSubMenu(new Menu("Draw", "DrawSettings"));
            Config.SubMenu("DrawSettings").AddItem(new MenuItem("Draw E", "E Range").SetValue(false));

            // Add menu
            Config.AddToMainMenu();
            Game.PrintChat("Drunk Tristana Provied by: DrunkFratBoy");
            Game.OnGameUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;

            //Test sdfsf/// dfsafdsafsfsfsa/ dsfsfsf

            throw new NotImplementedException(); // End of the function
        }


        static void Drawing_OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            // Add 9 + range per level
            Q.Range = 541 + 9 * (Player.Level - 1);
            E.Range = 541 + 9 * (Player.Level - 1);
            R.Range = 541 + 9 * (Player.Level - 1);
            
            if (Player.IsDead) return;

            KillSteal();

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                Combo();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
               // Harass();
            }

            // Add auto farm maybe?


            throw new NotImplementedException();
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            var sender = gapcloser.Sender;
            if (Config.Item("UseAntiGapcloser").GetValue<bool>() != true) return;

            else if (sender.IsValidTarget(R.Range))
            {
                R.CastOnUnit(sender);
            }
            
            throw new NotImplementedException();
        }

        static void KillSteal()
        {
            if (!Config.Item("UseRKS").GetValue<bool>() && !Config.Item("UseEKs").GetValue<bool>()) return;

            foreach (var target in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (Config.Item("UseEKs").GetValue<bool> () && !target.IsDead && E.IsReady() && !target.IsAlly && Player.Distance(target.Position) < E.Range && Player.GetSpellDamage(target,SpellSlot.E) > (target.Health + 20)) {
                    Game.PrintChat("Attempting to use E KillSteal");
                    E.CastOnUnit(target);
                }

                // Add R kill steal
            }
        }


        static void Combo()
        {
            var Target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
            var useQ = Config.Item("UseQC").GetValue<bool>();
            var useE = Config.Item("UseEC").GetValue<bool>();
            var useR = Config.Item("UseRC").GetValue<bool>();
            var hitR = Config.Item("UseRCombo1").GetValue<Slider>().Value;
            Game.PrintChat("Combo Active");

            if (Target == null) return;

            if (Q.IsReady() && useQ)
            {
                Q.Cast();
            }

            if (useE && E.IsReady())
            {
                E.Cast(Target);
            }

            if (!useR || !R.IsReady()) return;
            if (R.IsKillable(Target))
            {
                R.CastOnUnit(Target);
            }

        }
    }
}
