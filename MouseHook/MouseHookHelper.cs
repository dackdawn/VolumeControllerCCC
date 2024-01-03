using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MouseHook
{
    public partial class MouseHookHelper
    {
        private IntPtr _hMouseHook; // 键盘钩子返回值

        private readonly HookProc _mouseHookProcedure; // 鼠标钩子回调委托

        private event MouseEventHandler MouseEvent; // 供外部注册回调event

        public MouseHookHelper()
        {
            _hMouseHook = IntPtr.Zero;
            _mouseHookProcedure = MouseHookProc;
        }



        public void RegisterEvent(MouseEventHandler newEvent)
        {
            MouseEvent += newEvent;
        }

        public void UnRegisterEvent(MouseEventHandler existEvent)
        {
            MouseEvent -= existEvent;
        }

        public void Start()
        {
            // 安装键盘钩子
            if (_hMouseHook == IntPtr.Zero) {
                _hMouseHook = SetWindowsHookEx(
                                    HookType.WH_MOUSE_LL,
                                    _mouseHookProcedure,
                                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                                    0);

                //如果SetWindowsHookEx失败
                if (_hMouseHook == IntPtr.Zero) {
                    throw new Exception("安装键盘钩子失败");
                }
            }
        }

        public void Stop()
        {
            var retUnhook = true;


            if (_hMouseHook != IntPtr.Zero) {
                retUnhook = UnhookWindowsHookEx(_hMouseHook);
                _hMouseHook = IntPtr.Zero;
            }

            if (!retUnhook) {
                throw new Exception("卸载钩子失败！");
            }
        }
        
        private IntPtr MouseHookProc(
            int code,
            IntPtr wParam,
            IntPtr lParam)
        {
            // 全局鼠标事件回调
            if (code >= 0) {
                var wp = (WM)wParam;
                MouseClickEvents button;
                bool handle;
                switch (wp) {
                    case WM.MOUSEWHEEL:
                        button = MouseClickEvents.Wheel;
                        handle = true;
                        break;
                    case WM.LBUTTONDOWN:
                        button = MouseClickEvents.LeftDown;
                        handle = true;
                        break;
                    case WM.LBUTTONUP:
                        button = MouseClickEvents.LeftUp;
                        handle = true;
                        break;
                    case WM.MOUSEMOVE:
                        button = MouseClickEvents.Move;
                        handle = true;
                        break;
                    default:
                        button = MouseClickEvents.None;
                        handle = false;
                        break;
                }

                if (MouseEvent != null && handle) {
                    var myMouseStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    var e = new MouseData(button, myMouseStruct.pt.X, myMouseStruct.pt.Y,
                        myMouseStruct.mouseData, myMouseStruct.time);
                    MouseEvent(this, e);
                }
            }

            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        ~MouseHookHelper()
        {
            Stop();
        }
    }
}
