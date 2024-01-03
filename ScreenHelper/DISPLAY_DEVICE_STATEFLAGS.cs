using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenHelper
{
    public partial class ScreenHelper
    {
        private readonly uint DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001;
        private readonly uint DISPLAY_DEVICE_MULTI_DRIVER = 0x00000002;
        private readonly uint DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004;
        private readonly uint DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008;
        private readonly uint DISPLAY_DEVICE_VGA_COMPATIBLE = 0x00000010;
        private readonly uint DISPLAY_DEVICE_REMOVABLE = 0x00000020;
    }
}
