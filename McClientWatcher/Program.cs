using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McClientWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher fsw = new FileSystemWatcher();
            fsw.Path = "C:\\";
            fsw.Filter = "mcstarted.txt";
            fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            fsw.Created += fsw_Trigger;

            fsw.EnableRaisingEvents = true;

            Console.WriteLine("Press 'q' then 'enter' to quit monitoring the minecraft client.");
            while (Console.Read() != 'q') ;
        }

        static void fsw_Trigger(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Restart detected at " + DateTime.Now.ToString());
            Console.WriteLine("Killing existing minecraft clients.");
            KillMinecraftProcesses();
            Console.WriteLine("Starting minecraft client.");
            StartMinecraftClient();
            Console.WriteLine("Press 'q' then 'enter' to quit monitoring the minecraft client.");
        }


        private static void KillMinecraftProcesses()
        {
            // Kill any minecraft client apps.  They have a process name of "javaw",
            // whereas the minecraft server uses a process name of "javaw.
            var procs = from proc in Process.GetProcesses()
                        where proc.ProcessName == "javaw"
                        select proc;

            foreach (var proc in procs)
            {
                proc.Kill();
            }
        }

        private static void StartMinecraftClient()
        {
            var psi = new ProcessStartInfo();
            psi.WorkingDirectory = "D:\\Minecraft";
            psi.FileName = "minecraft.exe";
            psi.Arguments = "shortstack615 ricky123 mc.gordon-online.com:25545";
            Process.Start(psi);
        }
    }
}
