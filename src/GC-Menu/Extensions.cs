using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    static class Extensions
    {
        public static void ShowAndCenterOnActiveScreen(this ToolStripDropDown control, int menuWidth)
        {
            var mousePoint = Cursor.Position;
            var screen = Screen.FromPoint(mousePoint);

            var left = screen.Bounds.X + screen.Bounds.Width / 2 - control.Width / 2;

            if (mousePoint.X > left && mousePoint.X < (left + menuWidth)) // cursor is over menu so move menu otherwise cursor will pre-select the item under itself
                left = Cursor.Position.X + 10;

            var top = screen.Bounds.Y + screen.Bounds.Height / 2 - control.Height / 2;
            control.Show(new Point(left, top));
        }

        public static string ToHtmlClipboardData(this string htmlFragment)
        {
            const string header = "Version:0.9\r\n";
            string source = "<html><body><!--StartFragment-->" + htmlFragment + "<!--EndFragment--></body></html>";

            int startHtml = header.Length;
            int startFragment = source.IndexOf("<!--StartFragment-->") + "<!--StartFragment-->".Length;
            int endFragment = source.IndexOf("<!--EndFragment-->");
            int endHtml = source.Length;

            return string.Format(
                "{0}StartHTML:{1:00000000}\r\nEndHTML:{2:00000000}\r\nStartFragment:{3:00000000}\r\nEndFragment:{4:00000000}\r\n{5}",
                header,
                startHtml,
                endHtml,
                startFragment,
                endFragment,
                source
            );
        }
    }

    internal class Global
    {
        static Global() => Directory.CreateDirectory(DataDir);

        public static string DataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GlobalContextMenu");
        public static string ConfigFile = Path.Combine(Global.DataDir, "config.txt");
        public static string HotKeyFile = Path.Combine(Global.DataDir, "hotkey.txt");
    }
}