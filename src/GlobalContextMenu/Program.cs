using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GlobalContextMenu
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.FirstOrDefault() == "-menu")
            {
                new DispatcherForm().ShowDialog();
            }
            else
            {
                KillOtherInstances();

                var keySpec = Config.ReadHotkey();

                HotKeys.Instance.Start();
                HotKeys.Instance.Bind(keySpec, Program.OnHotKeyInvoked);

                Application.Run(new AppForm());

                HotKeys.Instance.Stop();
            }
        }

        public static void KillOtherInstances()
        {
            Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location))
                .Where(x => x.Id != Process.GetCurrentProcess().Id)
                .ToList()
                .ForEach(p =>
                {
                    try
                    {
                        p.Kill();
                    }
                    catch { }
                });
        }

        public static void OnHotKeyInvoked()
        {
            Process.Start(Assembly.GetExecutingAssembly().Location, "-menu");
        }
    }
}