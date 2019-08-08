using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    class Config
    {
        public static (string title, string text)[] MenuData
        {
            get
            {
                try
                {
                    EnsureDefaults();
                    return File.ReadAllLines(Global.ConfigFile)
                               .Where(x => !x.StartsWith(";"))
                               .Select(x =>
                               {
                                   var parts = x.Split("|".ToCharArray(), 2);
                                   if (parts.Count() == 1)
                                       return (parts[0], parts[0]);
                                   else
                                       return (parts[0], parts[1]);
                               })
                               .ToArray();
                }
                catch
                {
                    return new (string, string)[0];
                }
            }
        }

        public static void EnsureDefaults(string configFile = null)
        {
            configFile = configFile ?? Global.ConfigFile;

            if (!File.Exists(configFile))
                CreateDefaultConfigFile(configFile);
        }

        static string ConfigHeader =
@";  <display text>|<insertion text>
;  use '\n' to indicate the line breaks
;  Example: Hello...|Hello World!
;---------------------------------------";

        static void CreateDefaultConfigFile(string configFile)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(configFile));
            File.WriteAllText(configFile, ConfigHeader +
@"
Hello...|Hello World!
;---------------------------------------");
        }

        public static bool Edit(string configFile = null)
        {
            try
            {
                configFile = configFile ?? Global.ConfigFile;

                var timestamp = File.GetLastWriteTimeUtc(configFile);
                Process.Start("notepad.exe", configFile).WaitForExit();
                return timestamp != File.GetLastWriteTimeUtc(configFile);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Global Context Menu");
                return false;
            }
        }
    }
}