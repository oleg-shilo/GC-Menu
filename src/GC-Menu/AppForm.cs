using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
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
            notifyIcon1.Icon = Properties.resources.tray_icon;
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

        void convertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var thread = new Thread(DispatcherForm.ConvertHrtmlToTextClipboard);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() => Config.EditItems());
        }

        void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            hotKeyToolStripMenuItem_Click(sender, e);
        }
    }
}