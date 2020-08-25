using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    class Config
    {
        static Config()
        {
            if (!File.Exists(Global.HotKeyFile))
                File.WriteAllText(Global.HotKeyFile, "Ctrl+Shift+L");
        }

        static public string ReadHotkey() => File.ReadAllText(Global.HotKeyFile);

        static public void WriteHotkey(string hotKey) => File.WriteAllText(Global.HotKeyFile, hotKey);

        static string shortcutFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "GlobalContextMenu.lnk");

        static public bool IsAppStartupConfigured()
            => File.Exists(shortcutFile);

        static public void ConfigAppStartup(bool start)
        {
            try
            {
                if (start)
                {
                    Utils.CreateShortcut(shortcutFile, Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    if (File.Exists(shortcutFile))
                        File.Delete(shortcutFile);
                }
            }
            catch { } //doesn't matter why we failed, just ignore and continue
        }

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

        public static bool EditItems(string configFile = null)
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

        static class Utils
        {
            public static string CreateShortcut(string destFile, string appPath, string args = null)
            {
                Debug.Assert(destFile.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase));

                //need to do renaming as Shell Object doesn't support unicode file names
                var destinationFolder = Path.GetDirectoryName(destFile);
                var asciiDestFile = Path.Combine(destinationFolder, Guid.NewGuid() + ".lnk");

                dynamic shell = null;
                dynamic lnk = null;

                try
                {
                    shell = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
                    lnk = shell.CreateShortcut(asciiDestFile);
                    lnk.WorkingDirectory = Path.GetDirectoryName(appPath);
                    lnk.TargetPath = appPath;
                    lnk.Arguments = args;
                    lnk.Save();

                    if (File.Exists(destFile))
                        File.Delete(destFile);
                    File.Move(asciiDestFile, destFile);

                    return destFile;
                }
                finally
                {
                    if (lnk != null) Marshal.FinalReleaseComObject(lnk);
                    if (shell != null) Marshal.FinalReleaseComObject(shell);
                }
            }
        }
    }
}