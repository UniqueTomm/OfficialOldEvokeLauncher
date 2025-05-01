using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using IniParser;

namespace SteraLauncher.Services
{
    public static class UpdateINI
    {
        public static void WriteToConfig(string SectionName, string PathKey, string NewValue)
        {
            try
            {
                string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string DataFolder = Path.Combine(BaseFolder, "Evoke OGFN");
                Directory.CreateDirectory(DataFolder);
                string FilePath = Path.Combine(DataFolder, "Settings.ini");

                //Console.WriteLine($"[DEBUG] INI File Path: {FilePath}");

                FileIniDataParser parser = new FileIniDataParser();

                IniData iniData;
                if (File.Exists(FilePath))
                {
                    iniData = parser.ReadFile(FilePath);
                    //Console.WriteLine("[DEBUG] INI File Found. Loading existing data.");
                }
                else
                {
                    iniData = new IniData();
                }

                iniData[SectionName][PathKey] = NewValue;
                parser.WriteFile(FilePath, iniData);

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"[ERROR] Failed to write to INI file: {ex.Message}");
            }
        }


        public static string ReadValue(string SectionName, string PathKey)
        {
            string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string DataFolder = Path.Combine(BaseFolder, "Evoke OGFN");
            string FilePath = Path.Combine(DataFolder, "Settings.ini");

            FileIniDataParser parser = new FileIniDataParser();

            if (File.Exists(FilePath))
            {
                IniData iniData = parser.ReadFile(FilePath);

                return iniData[SectionName][PathKey];
            }
            else
            {
                return "NONE";
            }
        }

        public static void DeleteSection(string SectionName)
        {
            string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string DataFolder = Path.Combine(BaseFolder, "Evoke OGFN");
            string FilePath = Path.Combine(DataFolder, "Settings.ini");

            FileIniDataParser parser = new FileIniDataParser();

            if (File.Exists(FilePath))
            {
                IniData iniData = parser.ReadFile(FilePath);

                // Check if the section exists before attempting to delete
                if (iniData.Sections.ContainsSection(SectionName))
                {
                    iniData.Sections.RemoveSection(SectionName);
                    parser.WriteFile(FilePath, iniData, null);
                }
            }
        }
    }
}
