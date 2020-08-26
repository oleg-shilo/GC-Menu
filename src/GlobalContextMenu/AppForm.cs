using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

using System.Windows.Interop;

namespace GlobalContextMenu
{
    public partial class AppForm : Form
    {
        public AppForm()
        {
            this.Load += AppForm_Load;

            InitializeComponent();

            Opacity = 0;
        }

        void AppForm_Load(object sender, EventArgs e)
        {
            Task.Run(() => this.Invoke((Action)this.Hide));
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        void hotKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ConfigForm.PopUp())
            {
                var keySpec = File.ReadAllText(Global.HotKeyFile);

                HotKeys.Instance.Stop();
                HotKeys.Instance.Start();
                HotKeys.Instance.Bind(keySpec, Program.OnHotKeyInvoked);
            }
        }

        void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.EditItems();
        }

        void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            hotKeyToolStripMenuItem_Click(sender, e);
        }
    }
}