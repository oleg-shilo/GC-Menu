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
            var data = Config.MenuData;

            int lastLevel = 0;
            Dictionary<int, ToolStripItemCollection> lastGroupItem = new Dictionary<int, ToolStripItemCollection>()
            {
                {0,  cm.Items }
            };

            foreach (var item in data)
            {
                var parent = lastGroupItem[item.level];

                if (item.title == "---")
                    parent.Add(new ToolStripSeparator());
                else
                {
                    if (item.text == null)
                    {
                        parent.Add(item.title);
                        lastLevel = item.level + 1;
                        lastGroupItem[lastLevel] = parent.Cast<ToolStripItem>()
                                                         .Where(x => x is ToolStripMenuItem)
                                                         .Cast<ToolStripMenuItem>()
                                                         .Last().DropDownItems;
                    }
                    else
                    {
                        parent.Add(item.title, null, (sender1, e1) => SendInput(item.text.Replace("\\n", Environment.NewLine)));
                        lastLevel = item.level;
                    }
                }
            }

            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add("Organize items", null, (s, e) => Config.EditItems());

            var widestMenuItem = cm.Items.OfType<ToolStripItem>().Select(x => x.Text).OrderByDescending(x => x.Length).FirstOrDefault() ?? "";

            // var menuWidth = this.CreateGraphics().MeasureString(widestMenuItem, this.Font);
            var menuWidth = cm.Width + (cm.Padding.Horizontal * 2);

            cm.Closed += (s, e) => Close();
            cm.ShowAndCenterOnActiveScreen(menuWidth);

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