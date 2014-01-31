namespace Fusionbird.FusionToolkit
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.InteropServices;

    internal class NativeMethods
    {
        public const int NM_CUSTOMDRAW = -12;
        public const int NM_FIRST = 0;
        public const int S_OK = 0;
        public const int TMT_COLOR = 0xcc;

        private NativeMethods()
        {
        }

        [DllImport("UxTheme.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern int CloseThemeData(IntPtr hTheme);
        [DllImport("Comctl32.dll", EntryPoint="DllGetVersion", CallingConvention=CallingConvention.Cdecl)]
        public static extern int CommonControlsGetVersion(ref DLLVERSIONINFO pdvi);
        [DllImport("UxTheme.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);
        [DllImport("UxTheme.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern int GetThemeColor(IntPtr hTheme, int iPartId, int iStateId, int iPropId, ref int pColor);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("UxTheme.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool IsAppThemed();
        [DllImport("UxTheme.dll", CallingConvention=CallingConvention.Cdecl, CharSet=CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hwnd, string pszClassList);

        public enum CustomDrawDrawStage
        {
            CDDS_ITEM = 0x10000,
            CDDS_ITEMPOSTERASE = 0x10004,
            CDDS_ITEMPOSTPAINT = 0x10002,
            CDDS_ITEMPREERASE = 0x10003,
            CDDS_ITEMPREPAINT = 0x10001,
            CDDS_POSTERASE = 4,
            CDDS_POSTPAINT = 2,
            CDDS_PREERASE = 3,
            CDDS_PREPAINT = 1,
            CDDS_SUBITEM = 0x20000
        }

        public enum CustomDrawItemState
        {
            CDIS_CHECKED = 8,
            CDIS_DEFAULT = 0x20,
            CDIS_DISABLED = 4,
            CDIS_FOCUS = 0x10,
            CDIS_GRAYED = 2,
            CDIS_HOT = 0x40,
            CDIS_INDETERMINATE = 0x100,
            CDIS_MARKED = 0x80,
            CDIS_SELECTED = 1,
            CDIS_SHOWKEYBOARDCUES = 0x200
        }

        public enum CustomDrawReturnFlags
        {
            CDRF_DODEFAULT = 0,
            CDRF_NEWFONT = 2,
            CDRF_NOTIFYITEMDRAW = 0x20,
            CDRF_NOTIFYPOSTERASE = 0x40,
            CDRF_NOTIFYPOSTPAINT = 0x10,
            CDRF_NOTIFYSUBITEMDRAW = 0x20,
            CDRF_SKIPDEFAULT = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DLLVERSIONINFO
        {
            public int cbSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMCUSTOMDRAW
        {
            public Fusionbird.FusionToolkit.NativeMethods.NMHDR hdr;
            public Fusionbird.FusionToolkit.NativeMethods.CustomDrawDrawStage dwDrawStage;
            public IntPtr hdc;
            public Fusionbird.FusionToolkit.NativeMethods.RECT rc;
            public IntPtr dwItemSpec;
            public Fusionbird.FusionToolkit.NativeMethods.CustomDrawItemState uItemState;
            public IntPtr lItemlParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr HWND;
            public int idFrom;
            public int code;
            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture, "Hwnd: {0}, ControlID: {1}, Code: {2}", new object[] { this.HWND, this.idFrom, this.code });
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public RECT(Rectangle rect)
            {
                this = new Fusionbird.FusionToolkit.NativeMethods.RECT();
                this.Left = rect.Left;
                this.Top = rect.Top;
                this.Right = rect.Right;
                this.Bottom = rect.Bottom;
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", new object[] { this.Left, this.Top, this.Right, this.Bottom });
            }

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(this.Left, this.Top, this.Right, this.Bottom);
            }
        }

        public enum TrackBarCustomDrawPart
        {
            TBCD_CHANNEL = 3,
            TBCD_THUMB = 2,
            TBCD_TICS = 1
        }

        public enum TrackBarParts
        {
            TKP_THUMB = 3,
            TKP_THUMBBOTTOM = 4,
            TKP_THUMBLEFT = 7,
            TKP_THUMBRIGHT = 8,
            TKP_THUMBTOP = 5,
            TKP_THUMBVERT = 6,
            TKP_TICS = 9,
            TKP_TICSVERT = 10,
            TKP_TRACK = 1,
            TKP_TRACKVERT = 2
        }
    }
}

