using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace GetSpeedDash
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        static void Loading_OnLoadingComplete(EventArgs args)
        {
            Dash.OnDash += Dash_OnDash;
        }

        static bool saved;
        static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            Chat.Print(e.Speed);
            if (sender.IsMe && !saved)
            {
                Save(Player.Instance.ChampionName, e.Speed.ToString());               
                saved = true;
            }
            if (sender.IsMe && saved)
            {
                saved = false;
            }
        }
        public static void Save(string champname, string dashspeed)
        {            
            var sandboxConfig = EloBuddy.Sandbox.SandboxConfig.DataDirectory + @"\DashSpeed\";
            var xFile = sandboxConfig + @"\" + "Dash Infomation" + ".txt";

            if (!Directory.Exists(sandboxConfig))
            {
                Directory.CreateDirectory(sandboxConfig);
            }
            string[] data = { champname, dashspeed };
            File.AppendAllLines(xFile, data);
            //File.AppendText(xFile).WriteLine(champname);
            //File.AppendText(xFile).WriteLine(dashspeed);
            //File.AppendText(xFile).Dispose();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Saved - Check Directory");
            Console.ResetColor();
        }
    }
}
