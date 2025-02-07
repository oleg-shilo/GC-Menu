using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    public partial class ConfigForm : Form
    {
        public static bool PopUp()
        {
            bool result = false;
            if (!showing)
            {
                showing = true;
                result = new ConfigForm().ShowDialog() == DialogResult.OK;
                showing = false;
            }
            return result;
        }

        string initialhotKey;
        bool initialStartWithWindows;
        static bool showing;

        public ConfigForm()
        {
            InitializeComponent();
            initialStartWithWindows = startWithWindows.Checked = Config.IsAppStartupConfigured();
            initialhotKey = hotKey.Text = Config.ReadHotkey();
            this.Load += ConfigForm_Load;
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Settings (GC-Menu v{this.GetType().Assembly.GetName().Version})";
            SetForegroundWindow(this.Handle);
        }

        void OK_Click(object sender, EventArgs e)
        {
            if (initialhotKey != hotKey.Text)
            {
                Config.WriteHotkey(hotKey.Text);
                DialogResult = DialogResult.OK;
            }

            if (initialStartWithWindows != startWithWindows.Checked)
            {
                Config.ConfigAppStartup(startWithWindows.Checked);
                DialogResult = DialogResult.OK;
            }
        }

        string hotKeyInput;

        private void hotKey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var modifiers = "";

            if (e.Modifiers.HasFlag(Keys.Control))
                modifiers += "Ctrl+";

            if (e.Modifiers.HasFlag(Keys.Shift))
                modifiers += "Shift+";

            if (e.Modifiers.HasFlag(Keys.Alt))
                modifiers += "Alt+";

            var key = e.KeyCode.ToString();

            if (key.EndsWith("ShiftKey") || key.EndsWith("ControlKey") || key.EndsWith("Menu"))
                hotKey.Text = "";
            else
                hotKey.Text = modifiers + key;
        }

        private void hotKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", Path.GetDirectoryName(Config.shortcutFile));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var t = Clipboard.GetDataObject(); // this is needed to prevent clipboard from being locked
        }
    }
}