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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    public partial class DispatcherForm : Form
    {
        IntPtr parent;

        public DispatcherForm()
        {
            parent = GetForegroundWindow();
            InitializeComponent();

            this.Activated += (s, e) => Popup();
        }

        void Popup()
        {
            this.Hide();
            Config.EnsureDefaults();

            var cm = new ContextMenuStrip();
            foreach (var item in Config.MenuData)
                if (item.title.Trim() == "---")
                    cm.Items.Add(new ToolStripSeparator());
                else
                    cm.Items.Add(item.title, null, (sender1, e1) => SendInput(item.text.Replace("\\n", Environment.NewLine)));

            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add("Organize items", null, (s, e) => Config.EditItems());

            cm.Closed += (s, e) => Close();
            cm.ShowAndCenterOnActiveScreen();

            cm.Focus();
        }

        void SendInput(string text)
        {
            SetForegroundWindow(parent);
            SendKeys.Flush();
            Thread.Sleep(10);
            text.ToList().ForEach(x =>
            {
                try
                {
                    Thread.Sleep(10);
                    if ("+^%~{}".Contains(x))
                        SendKeys.SendWait("{" + x + "}");
                    else
                        SendKeys.SendWait(x.ToString());
                    SendKeys.Flush();
                }
                catch { }
            });

            // SendKeys.SendWait(text); // works more reliable when sent one by one
            Close();
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32.dll")]
        static extern IntPtr GetForegroundWindow();
    }
}