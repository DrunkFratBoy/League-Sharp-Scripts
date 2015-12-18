using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Drawing;

using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace FratsYI
{
    internal enum Spells
    {
        Q, W, E , R
    }
    
    internal static class MasterYi
    {
        #region Static Fields
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public static Orbwalking.Orbwalker Orbwalker;
        public static Menu Menu;
        public static int lastMov;

        public static Dictionary<Spells, Spell> spells = new Dictionary<Spells, Spell>
                                                             {
                                                                 { Spells.Q, new Spell(SpellSlot.Q, 600f) },
                                                                 { Spells.W, new Spell(SpellSlot.W) },
                                                                 { Spells.E, new Spell(SpellSlot.E) }, 
                                                                 { Spells.R, new Spell(SpellSlot.R) }
                                                             };

        #endregion

        public static void OnLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharData.BaseSkinName != "MasterYi")
            {
                return;
            }

#region Menu
            // Create Menu
            Menu = new Menu("FratYi",Player.ChampionName, true);

            // Add orbwalker to Submenu
            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            // Add target selctor
            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);

            // Add Combo Menu
            Menu comboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            comboMenu.AddItem(new MenuItem("UseQCombo", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseWCombo", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseECombo", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseRCombo", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("UseWAggresive", "Use W for Auto Attack Reset").SetValue(true));


            // Add in the Farm Menu
            Menu farmMenu = Menu.AddSubMenu(new Menu("Farm", "Farm"));
            farmMenu.AddItem(new MenuItem("UseQFarm", "Use Q").SetValue(true));
           


            // Add Menu to the Shift Menu
            Menu.AddToMainMenu();

            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;

            Notifications.AddNotification("Welcome to FratsYI", 5000);





            #endregion
        }

        private static void OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();

                    var manaUse = Player.Mana * 100 / Player.MaxMana;

                    if (manaUse >= Menu.Item("Qmana").GetValue<Slider>().Value)
                    {
                        LaneClear();
                    }
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;

            }
        }

        #region Methods
        private static void Drawing_OnDraw(EventArgs args)
        {
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {

        }


        // xQx LaneClear Thanks!
        private static void LaneClear()
        {
            var useQ = Menu.Item("UseQFarm").GetValue<bool>();
            var allMinions = MinionManager.GetMinions(
                Player.ServerPosition, spells[Spells.Q].Range, MinionTypes.All, MinionTeam.NotAlly);

            if (spells[Spells.Q].IsReady() && useQ)
            {
                var closestMinion = new Obj_AI_Base();
                if (allMinions.Any())
                {
                    foreach (var minion in allMinions)
                    {
                        if (allMinions.IndexOf(minion) == 0)
                        {
                            closestMinion = minion;
                        }
                        else if (Player.Distance(minion.Position) < Player.Distance(closestMinion.Position))
                        {
                            closestMinion = minion;
                        }
                    }
                    if (!closestMinion.IsValidTarget())
                        return;

                    spells[Spells.Q].Cast(closestMinion);
                }
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(spells[Spells.Q].Range, TargetSelector.DamageType.Physical);
            var useQ = Menu.Item(("UseQCombo")).GetValue<bool>();
            var useE = Menu.Item(("UseECombo")).GetValue<bool>();
            var useW = Menu.Item(("UseWCombo")).GetValue<bool>();
            var useWAggresive = Menu.Item(("UseWAggresive")).GetValue<bool>();

           
            var useR = Menu.Item(("UseRCombo")).GetValue<bool>();

            if (spells[Spells.Q].IsReady() && useQ && target != null)
            {
                spells[Spells.Q].CastOnUnit(target);
            }

            bool inAARange = Orbwalking.InAutoAttackRange(target);
            if (useWAggresive && inAARange)
            {
                spells[Spells.W].Cast();
                
                if (Player.HasBuff("Meditate") && Environment.TickCount - lastMov > 200)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                    lastMov = Environment.TickCount;
                }

            }
            

            if (Orbwalker.InAutoAttackRange(target) && spells[Spells.E].IsReady() && useE)
            {
                spells[Spells.E].Cast(Player);
            }
            if (Orbwalker.InAutoAttackRange(target) && spells[Spells.W].IsReady() && useW)
            {
                spells[Spells.W].Cast(Player);
                
            }

            if (spells[Spells.R].IsReady() && useR && target != null)
            {
                spells[Spells.R].CastOnUnit(Player);
            }

        }

        private static void Harass()
        {

        }
        #endregion


    }
}
