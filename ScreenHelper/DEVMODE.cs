using System;
using System.Runtime.InteropServices;

namespace ScreenHelper
{
    public partial class ScreenHelper
    {
        /// <summary>
        /// only used for monitor. If you want the DEVMODE for printer, you should rewrite it.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        /// <summary>
        /// Due to align, this struct can't construct.
        /// todo: fix this.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct __DEVMODE_NO_LONGER_USE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;
        
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [FieldOffset(0)]
            public string dmDeviceName;
            [FieldOffset(32)]
            public Int16 dmSpecVersion;
            [FieldOffset(34)]
            public Int16 dmDriverVersion;
            [FieldOffset(36)]
            public Int16 dmSize;
            [FieldOffset(38)]
            public Int16 dmDriverExtra;
            [FieldOffset(40)]
            public Int32 dmFields;
        
            #region nameless union
        
            /// <summary>
            /// union for monitor or printer
            /// byte range: 44-69
            /// </summary>
        
            // printer
            [FieldOffset(44)]
            public Int16 dmOrientation;
            [FieldOffset(46)]
            public Int16 dmPaperSize;
            [FieldOffset(48)]
            public Int16 dmPaperLength;
            [FieldOffset(50)]
            public Int16 dmPaperWidth;
            [FieldOffset(52)]
            public Int16 dmScale;
            [FieldOffset(54)]
            public Int16 dmCopies;
            [FieldOffset(56)]
            public Int16 dmDefaultSource;
            [FieldOffset(58)]
            public Int16 dmPrintQuality;
        
            // monitor
            [FieldOffset(44)]
            public POINTL dmPosition;
            [FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;
        
            #endregion
        
        
            [FieldOffset(60)]
            public short dmColor;
            [FieldOffset(62)]
            public short dmDuplex;
            [FieldOffset(64)]
            public short dmYResolution;
            [FieldOffset(66)]
            public short dmTTOption;
            [FieldOffset(68)]
            public short dmCollate;
            [FieldOffset(70)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [FieldOffset(102)]
            public Int16 dmLogPixels;
            [FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [FieldOffset(108)]
            public Int32 dmPelsWidth;
            [FieldOffset(112)]
            public Int32 dmPelsHeight;
        
        
            #region nameless union
        
            /// <summary>
            /// union for 
            /// </summary>
        
            [FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [FieldOffset(116)]
            public Int32 dmNup;
        
            #endregion
        
        
            [FieldOffset(120)]
            public Int32 dmDisplayFrequency;
            [FieldOffset(124)]
            public Int32 dmICMMethod;
            [FieldOffset(128)]
            public Int32 dmICMIntent;
            [FieldOffset(132)]
            public Int32 dmMediaType;
            [FieldOffset(136)]
            public Int32 dmDitherType;
            [FieldOffset(140)]
            public Int32 dmReserved1;
            [FieldOffset(144)]
            public Int32 dmReserved2;
            [FieldOffset(148)]
            public Int32 dmPanningWidth;
            [FieldOffset(152)]
            public Int32 dmPanningHeight;
        }
    }
}
