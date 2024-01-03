using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScreenHelper
{
    public enum DirectionEnum
    {
        None = 0,
        Top = 1,
        Right = 2,
        Bottom = 3,
        Left = 4
    }
    public class ScreenInfo
    {
        public RECT MonitorPhysics;
        public RECT MonitorLogic;
        public RECT TaskBarPhysics;
        public RECT TaskBarLogic;
        public DirectionEnum TaskBarDirection;
    }
}
