using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;


namespace FirstWordAddIn
{
    /// <summary> 
    /// 这个类可以让你得到一个在运行中程序的所有鼠标和键盘事件 (系统钩子)
    /// 并且引发一个带KeyEventArgs和MouseEventArgs参数的.NET事件以便你很容易使用这些信息
    /// 鼠标钩子处理函数：MouseHookProc
    /// 键盘钩子处理函数：KeyboardHookProc
    /// 使用方法：
    ///   1.新建该类。
    ///   2.声明OnMouseActivity、OnKeyDownEvent、OnKeyUpEvent、OnKeyPressEvent.
    ///   3.使用时调用相应的事件即可。
    ///   4.要添加应用请在MouseHookProc()和KeyboardHookProc()中自己添加。
    /// 使用示例：
    /// public mkhook = new KeyBordHook();
    /// mkhook.OnKeyDownEvent += new KeyEventHandler(OnKeyDownEventHandler);
    /// mkhook.OnMouseActivity += new MouseEventHandler(OnMouseEventHandler);
    /// private void OnKeyDownEventHandler(object sender, KeyEventArgs e)
    /// {
    ///     Console.WriteLine("KeyDown......");
    /// }
    /// private void OnMouseEventHandler(object sender, MouseEventArgs e)
    /// {
    ///     Console.WriteLine("MouseEvent......");
    /// }
    /// </summary> 
    /// <remarks> 
    /// 修改:xyan nay6@163.com
    /// 修改时间:10.06.11 
    /// </remarks> 
    public class KeyBordHook : IDisposable
    {
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        //全局的事件 
        public event MouseEventHandler OnMouseActivity;//鼠标事件
        public event KeyEventHandler OnKeyDownEvent;//键按下
        public event KeyEventHandler OnKeyUpEvent;//键放下
        public event KeyPressEventHandler OnKeyPressEvent;//键按下

        static int hMouseHook = 0;   //鼠标钩子句柄
        static int hKeyboardHook = 0; //键盘钩子句柄

        //鼠标常量 
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14; //鼠标常量 
        public const int WH_KEYBOARD_LL = 13; //键盘常量 

        HookProc MouseHookProcedure;   //声明鼠标钩子事件类型.
        HookProc KeyboardHookProcedure; //声明键盘钩子事件类型.

        //Declare   wrapper   managed   POINT   class. 
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }

        //声明鼠标钩子的封送结构类型 
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //声明键盘钩子的封送结构类型 
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode; //表示一个在1到254间的虚似键盘码 
            public int scanCode; //表示硬件扫描码 
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        //装置钩子的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //卸下钩子的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //下一个钩挂的函数 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        //先前按下的键
        public List<Keys> preKeys = new List<Keys>();

        /// <summary> 
        /// 墨认的构造函数构造当前类的实例并自动的运行起来. 
        /// </summary> 
        public KeyBordHook()
        {
            Start();
        }

        //析构函数. 
        ~KeyBordHook()
        {
            Stop();
        }

        public void Dispose()
        {
            Stop();
        }

        private void Start()
        {
            //安装键盘钩子 
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;

                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL,
                    KeyboardHookProcedure,
                    GetModuleHandle(curModule.ModuleName),
                    0);

                if (hKeyboardHook == 0)
                {
                    Stop();
                    Console.WriteLine("SetWindowsHookEx ist failed.");
                    throw new Exception("SetWindowsHookEx ist failed.");
                }
            }

            //   install   Mouse   hook   
            //if (hMouseHook == 0)
            //{
            //    //   Create   an   instance   of   HookProc. 
            //    MouseHookProcedure = new HookProc(this.MouseHookProc);

            //    Process curProcess = Process.GetCurrentProcess();
            //    ProcessModule curModule = curProcess.MainModule;
            //    //curProcess.Threads[0].Id;

            //    hMouseHook = SetWindowsHookEx(WH_MOUSE_LL,
            //        MouseHookProcedure,
            //        GetModuleHandle(curModule.ModuleName),
            //        0);//curProcess.Id//Process.GetCurrentProcess().Id

            //    //If   SetWindowsHookEx   fails. 
            //    if (hMouseHook == 0)
            //    {
            //        Stop();
            //        Console.WriteLine("SetWindowsHookEx   failed. ");
            //        throw new Exception("SetWindowsHookEx   failed. ");
            //    }
            //}
        }

        private void Stop()
        {
            bool retMouse = true;
            bool retKeyboard = true;

            //if (hMouseHook != 0)
            //{
            //    retMouse = UnhookWindowsHookEx(hMouseHook);
            //    hMouseHook = 0;
            //}

            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            //如果卸下钩子失败 
            if (!(retKeyboard))
            {
                Console.WriteLine("UnhookWindowsHookEx failed.");
                throw new Exception("UnhookWindowsHookEx failed.");
            }
        }

        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;

        //private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        //{
        //    //   if   ok   and   someone   listens   to   our   events 
        //    if ((nCode >= 0) && (OnMouseActivity != null))
        //    {
        //        MouseButtons button = MouseButtons.None;
        //        switch (wParam)
        //        {
        //            case WM_LBUTTONDOWN://鼠标左键
        //            case WM_LBUTTONUP:
        //            case WM_LBUTTONDBLCLK:
        //                button = MouseButtons.Left;
        //                break;
        //            case WM_RBUTTONDOWN://鼠标右键
        //            case WM_RBUTTONUP:
        //            case WM_RBUTTONDBLCLK:
        //                button = MouseButtons.Right;
        //                break;
        //            //case WM_MBUTTONDOWN:
        //            //    //case   WM_MBUTTONUP:   
        //            //    //case   WM_MBUTTONDBLCLK:   
        //            //    button = MouseButtons.Middle;
        //            //    break;
        //            default:
        //                break;
        //        }
        //        int clickCount = 0;
        //        if (button != MouseButtons.None)
        //        {
        //            if (wParam == WM_LBUTTONDBLCLK || wParam == WM_RBUTTONDBLCLK || wParam == WM_MBUTTONDBLCLK)
        //            {
        //                clickCount = 2;
        //            }
        //            else
        //            {
        //                clickCount = 1;
        //            }
        //        }
        //        //}
        //        //Console.WriteLine(clickCount.ToString());
        //        //Marshall   the   data   from   callback. 
        //        MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
        //        MouseEventArgs e = new MouseEventArgs(
        //        button,
        //        clickCount,
        //        MyMouseHookStruct.pt.x,
        //        MyMouseHookStruct.pt.y,
        //        0);
        //        OnMouseActivity(this, e);
        //    }
        //    return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        //}

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //Console.WriteLine("In KeyboardHookProc.");

            if ((nCode >= 0) && (OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键增加                   
                if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    if (IsCtrlAltShiftKeys(keyData) && preKeys.IndexOf(keyData) == -1)
                    {
                        preKeys.Add(keyData);
                        //Console.WriteLine(keyData.ToString());
                    }
                }
                //引发OnKeyDownEvent 
                if (OnKeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));

                    OnKeyDownEvent(this, e);
                }

                //引发OnKeyPressEvent 
                if (OnKeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.vkCode,
                    MyKeyboardHookStruct.scanCode,
                    keyState,
                    inBuffer,
                    MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        OnKeyPressEvent(this, e);
                    }
                }

                //当有OnKeyDownEvent 或 OnKeyPressEvent不为null时,ctrl alt shift keyup时 preKeys
                //中的对应的键删除
                if ((OnKeyDownEvent != null || OnKeyPressEvent != null) && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    if (IsCtrlAltShiftKeys(keyData))
                    {

                        for (int i = preKeys.Count - 1; i >= 0; i--)
                        {
                            if (preKeys[i] == keyData)
                            {
                                preKeys.RemoveAt(i);
                            }
                        }

                    }
                }
                //引发OnKeyUpEvent 
                if (OnKeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(GetDownKeys(keyData));
                    OnKeyUpEvent(this, e);
                }
            }
            //Console.WriteLine("Out KeyboardHookProc.");
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }



        private Keys GetDownKeys(Keys key)
        {
            Keys rtnKey = Keys.None;
            foreach (Keys keyTemp in preKeys)
            {
                switch (keyTemp)
                {
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        rtnKey = rtnKey | Keys.Control;
                        break;
                    case Keys.LMenu:
                    case Keys.RMenu:
                        rtnKey = rtnKey | Keys.Alt;
                        break;
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        rtnKey = rtnKey | Keys.Shift;
                        break;
                    default:
                        break;
                }
            }
            rtnKey = rtnKey | key;

            return rtnKey;
        }

        private Boolean IsCtrlAltShiftKeys(Keys key)
        {

            switch (key)
            {
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return true;
                default:
                    return false;
            }
        }
    }

}
