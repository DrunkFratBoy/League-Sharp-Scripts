using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Drunk_Fizz
{
    class Program
    {
        static void Main(string[] args)
        {
           CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        public static Spell Q, W, E, R;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Menu Config;
        private static Items.Item leetbb;
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        
        

        private static void Game_OnGameLoad(EventArgs args)
        {
            Q = new Spell(SpellSlot.Q, 1175); // DarkBinding

            
            Config = new Menu("Drunk-Morgana", "Drunk Morgana", true);
            
            Config.AddToMainMenu();
        }
    }
}
