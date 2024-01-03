using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseHook
{
    public partial class MouseHookHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public UIntPtr dwExtraInfo; // 指定额外信息相关的信息
            public KBDLLHOOKSTRUCTFlags flags; // 键标志
            public uint scanCode; // 指定的硬件扫描码的关键
            public uint time; // 指定的时间戳记的这个讯息
            public uint vkCode; //定一个虚拟键码。该代码必须有一个价值的范围1至254
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80
        }
    }
}
