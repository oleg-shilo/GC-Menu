using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    static class Extensions
    {
        public static void ShowAndCenterOnActiveScreen(this ToolStripDropDown control)
        {
            var screen = Screen.FromPoint(Cursor.Position);
            var left = screen.Bounds.X + screen.Bounds.Width / 2 - control.Width / 2;
            var top = screen.Bounds.Y + screen.Bounds.Height / 2 - control.Height / 2;
            control.Show(new Point(left, top));
        }
    }

    internal class Global
    {
        static Global() => Directory.CreateDirectory(DataDir);

        public static string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GlobalContextMenu");
        public static string ConfigFile = Path.Combine(Global.DataDir, "config.txt");
        public static string HotKeyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "hotkey.txt");
    }
}