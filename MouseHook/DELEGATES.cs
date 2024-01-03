using System;

namespace MouseHook
{
    public partial class MouseHookHelper
    {
        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

        public delegate void MouseEventHandler(object sender, MouseData e);
    }
}
