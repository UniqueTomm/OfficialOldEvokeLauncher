using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SteraLauncher.Services
{
    public static class PSBasics
    {
        public static Process _FortniteProcess;
        public static void Start(string PATH, string args, string Email, string Password)
        {

            string fortnitePath = UpdateINI.ReadValue("Paths", "FortnitePath");
            if (Email == null || Password == null)
            {
                MessageBox.Show("You have to log in with your account!");
                return;
            }
            if (File.Exists(Path.Combine(fortnitePath, @"EvokeAC.exe")));
            {
                PSBasics._FortniteProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = $"-AUTH_LOGIN={Email} -AUTH_PASSWORD={Password} -AUTH_TYPE=epic " + args,
                        FileName = Path.Combine(fortnitePath, @"EvokeAC.exe")
                    },
                    EnableRaisingEvents = true
                };
                PSBasics._FortniteProcess.Exited += new EventHandler(PSBasics.OnFortniteExit);
                PSBasics._FortniteProcess.Start();


            }

        }

        public static void OnFortniteExit(object sender, EventArgs e)
        {
            Process fortniteProcess = PSBasics._FortniteProcess;
            if (fortniteProcess != null && fortniteProcess.HasExited)
            {
                PSBasics._FortniteProcess = (Process)null;
            }
        }
    }
}
