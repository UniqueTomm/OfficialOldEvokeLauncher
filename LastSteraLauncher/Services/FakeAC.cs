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
    public class FakeAC
    {
        public static Process _FNLauncherProcess;
        public static Process _FNAntiCheatProcess;

        public static void Start(string Path69, string FileName, string args = "", string t = "r")
        {
            string fortnitePath = UpdateINI.ReadValue("Paths", "FortnitePath");
            try
            {
                if (File.Exists(Path.Combine(fortnitePath, "FortniteGame\\Binaries\\Win64\\", FileName)))
                {
                    ProcessStartInfo ProcessIG = new ProcessStartInfo()
                    {
                        FileName = Path.Combine(fortnitePath, "FortniteGame\\Binaries\\Win64\\", FileName),
                        Arguments = args,
                        CreateNoWindow = true,
                    };

                    if (t == "r")
                    {
                        _FNAntiCheatProcess = Process.Start(ProcessIG);
                        if (_FNAntiCheatProcess.Id == 0)
                        {
                            MessageBox.Show("FAILED STARTING!?!?!");
                        };
                    }
                    else
                    {
                        _FNLauncherProcess = Process.Start(ProcessIG);
                        if (_FNLauncherProcess.Id == 0)
                        {
                            MessageBox.Show("FAILED STARTING!?!?!");
                        };
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("THERE BEEN A ERROR");
            }
        }
    }
}
