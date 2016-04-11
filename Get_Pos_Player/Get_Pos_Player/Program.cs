using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Drawing;

namespace Get_Pos_Player
{
    class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }
        private static void OnLoad(EventArgs args)
        {
            Drawing.OnDraw += OnDraw;
        }
        private static void OnDraw(EventArgs args)
        {
            var mpos = Player.Instance.Position;
            var monster = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsMonster && x.IsValidTarget(600)).OrderBy(x => x.Health).FirstOrDefault();
            Drawing.DrawText(Drawing.Width - 200, 100, Color.White, "X: " + mpos.X);
            Drawing.DrawText(Drawing.Width - 200, 120, Color.White, "Y: " + mpos.Y);
            Drawing.DrawText(Drawing.Width - 200, 140, Color.White, "Z: " + mpos.Z);
        }
    }
}
