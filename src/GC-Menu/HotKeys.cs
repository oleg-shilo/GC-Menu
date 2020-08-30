using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

public class HotKeys
{
    static HotKeys instance;

    static public HotKeys Instance
    {
        get
        {
            return instance ?? (instance = new HotKeys());
        }
    }

    Dictionary<int, List<Action>> handlers = new Dictionary<int, List<Action>>();

    [DllImport("User32.dll")]
    static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

    [DllImport("User32.dll")]
    static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

    HwndSource source;
    Window wnd;

    HotKeys() //private
    {
    }

    void Init()
    {
        wnd = new Window
        {
            Width = 0,
            Height = 0,
            ShowInTaskbar = false,
            WindowStyle = WindowStyle.None
        };

        wnd.SourceInitialized += (s, e) =>
        {
            source = HwndSource.FromHwnd(wnd.GetSafeHandle());
            source.AddHook(HwndHook);
        };

        wnd.Loaded += (s, e) =>
        {
            wnd.MakeInvisible();
        };

        wnd.Closing += (s, e) =>
        {
            UnregisterAll();
            if (source != null)
            {
                source.RemoveHook(HwndHook);
                source = null;
            }
        };
    }

    public bool Started
    {
        get { return wnd != null && wnd.IsVisible; }
    }

    public void Start()
    {
        if (wnd == null)
            Init();

        wnd.Show();
    }

    public void Stop()
    {
        wnd.Close();
        wnd = null;
    }

    public int Bind(string keySpec, Action handler)
    {
        var (modifiers, key) = keySpec.ToHotKey();

        return HotKeys.Instance.Bind(modifiers, key, handler);
    }

    public int Bind(Modifiers modifiers, Keys key, Action handler)
    {
        if (source == null)
        {
            //throw new Exception("HotKey object isn't initialized yet. You need to call HotKeys.Start before setting any binding.");
            return 0;
        }
        else
        {
            int id = string.Format("{0}:{1}", key, modifiers).GetHashCode();

            if (RegisterHotKey(source.Handle, id, (uint)modifiers, (uint)key))
            {
                if (!handlers.ContainsKey(id))
                    handlers[id] = new List<Action>();

                handlers[id].Add(handler);

                return id;
            }
            else
                return -1;
        }
    }

    public void UnregisterAll()
    {
        if (Started)
        {
            var hWnd = wnd.GetSafeHandle();

            foreach (var hotKey in handlers.Keys)
                UnregisterHotKey(hWnd, hotKey);

            handlers.Clear();
        }
    }

    IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        switch (msg)
        {
            case WM_HOTKEY:
                {
                    var id = wParam.ToInt32();
                    if (handlers.ContainsKey(id))
                    {
                        foreach (Action handler in handlers[id])
                            try { handler(); } catch { }
                        handled = true;
                        break;
                    }
                }
                break;
        }
        return IntPtr.Zero;
    }
}

static class WindowdExtensions
{
    public static void CentreOnActiveScreen(this Window window, double horizontalShift = 0, double verticalShift = 0)
    {
        var screen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);
        window.Left = screen.Bounds.X + ((screen.Bounds.Width - window.ActualWidth) / 2) + horizontalShift;
        window.Top = screen.Bounds.Y + ((screen.Bounds.Height - window.ActualHeight) / 2) + verticalShift;
    }

    static public IntPtr GetSafeHandle(this Window window) => new WindowInteropHelper(window).Handle;

    [Flags]
    public enum ExtendedWindowStyles
    {
        WS_EX_TOOLWINDOW = 0x00000080,
    }

    public enum GetWindowLongFields
    {
        GWL_EXSTYLE = (-20),
    }

    /// <summary>
    /// Hides the window from taskbar and app switch (Alt+Tab).
    /// Based on: http://stackoverflow.com/questions/357076/best-way-to-hide-a-window-from-the-alt-tab-program-switcher/551847#551847
    /// </summary>
    /// <param name="wnd">The WND.</param>
    public static void MakeInvisible(this Window wnd)
    {
        var wndHelper = new WindowInteropHelper(wnd);

        int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

        exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
        SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

    public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
        var error = 0;
        var result = IntPtr.Zero;

        // Win32 SetWindowLong doesn't clear error on success
        SetLastError(0);

        if (IntPtr.Size == 4)
        {
            Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
            error = Marshal.GetLastWin32Error();
            result = new IntPtr(tempResult);
        }
        else
        {
            result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
            error = Marshal.GetLastWin32Error();
        }

        if ((result == IntPtr.Zero) && (error != 0))
        {
            throw new System.ComponentModel.Win32Exception(error);
        }

        return result;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
    static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
    static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

    static int IntPtrToInt32(IntPtr intPtr)
    {
        return unchecked((int)intPtr.ToInt64());
    }

    [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
    public static extern void SetLastError(int dwErrorCode);

    static public (Modifiers, Keys) ToHotKey(this string hotkey)
    {
        string[] tokens = hotkey.Trim().ToMachineHotKey().Split('+').Select(x => x.Trim()).ToArray();

        Keys key = tokens.Last().ToKeys();

        Modifiers modifiers = tokens.Take(tokens.Length - 1)
                                    .ToModifiers();

        return (modifiers, key);
    }
}

static class StringExtensions
{
    public static System.Windows.Forms.Keys ToKeys(this string keyValue)
    {
        if (keyValue.Length == 1)
            keyValue = keyValue.ToUpper();
        return (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), keyValue);
    }

    public static Modifiers ToModifiers(this IEnumerable<string> keyValues)
    {
        Modifiers modifiers = 0;
        foreach (string m in keyValues)
            modifiers |= (Modifiers)Enum.Parse(typeof(Modifiers), m);
        return modifiers;
    }

    public static string ToMachineHotKey(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;
        else
            return text.Replace("Ctrl", "Control")
                   .Replace("PrtScr", "PrintScreen")
                   .Replace("Tilde", "Oemtilde"); //don't bother with Oem3
    }
}

[Flags]
public enum Modifiers
{
    Alt = 1,
    Control = 2,
    Shift = 4,
    Win = 8
}