using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Unmanaged
    {
        public const short SWP_NOMOVE = 0X2;
        public const short SWP_NOSIZE = 1;
        public const short SWP_NOZORDER = 0X4;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int GWL_STYLE = -16;
        public const long WS_VISIBLE = 0x10000000L;
        public const long WS_POPUP = 0x80000000L;

        //
        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;
        public const int SC_HSCROLL = 0xF080;

        //
        public const long MB_ICONQUESTION = 0x00000020L;
        public const long MB_YESNO = 0x00000004L;
        public const long IDYES = 6;
        public const long IDNO = 7;

        [DllImport("user32.dll")]
        public static extern int MessageBoxA(IntPtr hWnd, string message, string title, long type);

        [DllImport("user32.dll")]
        public static extern bool SetWindowTextA(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int mode);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr HWnd, int insertAfter, int x, int y, int cx, int xy, int flags);

        [DllImport("user32.dll")]
        public static extern bool SetWindowLongPtr(IntPtr HWnd, int nIndex, long flags);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        public static readonly IntPtr ThisConsole = GetConsoleWindow();

        //
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll")]
        private static extern SafeFileHandle CreateFile(string filename, uint fileaccess, uint fileshare,
            IntPtr securityAttributes, FileMode creationDisposition, int flags, IntPtr template);

        private static readonly SafeFileHandle FileHandle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        [DllImport("kernel32.dll")]
        private static extern bool WriteConsoleOutput(SafeFileHandle hConsoleOutput, CharInfo[] lpBuffer, SmallCoord dwBufferSize,
            SmallCoord dwBufferCoord, ref SmallRect lpWriteRegion);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        public static int SetColor(ConsoleColor consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }
        const int STD_OUTPUT_HANDLE = -11;
        public static int SetColor(ConsoleColor color, uint r, uint g, uint b)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }

            switch (color)
            {
                case ConsoleColor.Black:
                    csbe.black = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    csbe.darkBlue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    csbe.darkGreen = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    csbe.darkCyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    csbe.darkRed = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    csbe.darkMagenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    csbe.darkYellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    csbe.gray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    csbe.darkGray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    csbe.blue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    csbe.green = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    csbe.cyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    csbe.red = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    csbe.magenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    csbe.yellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    csbe.white = new COLORREF(r, g, b);
                    break;
            }
            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }

        public static void RegionWrite(CharInfo[] image, int x, int y, int width, int height)
        {
            if (!FileHandle.IsInvalid)
            {
                int length = width * height;

                short sx = (short)x;
                short sy = (short)y;
                short swidth = (short)width;
                short sheight = (short)height;

                // Make a buffer size out our dimensions
                SmallCoord bufferSize = new SmallCoord(swidth, sheight);

                // Not really sure what this is but its probably important
                SmallCoord pos = new SmallCoord(0, 0);

                // Where do we place this?
                SmallRect rect = new SmallRect() { Left = sx, Top = sy, Right = (short)(sx + swidth), Bottom = (short)(sy + sheight) };

                bool b = WriteConsoleOutput(FileHandle, image, bufferSize, pos, ref rect);
            }
            else
            {
                throw new Exception("Console handle is invalid.");
            }

        }
    }
    public struct SmallCoord
    {
        public short X;
        public short Y;

        public SmallCoord(short x, short y)
        {
            X = x; Y = y;
        }

    };
    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo
    {
        [FieldOffset(0)] public char Char;
        [FieldOffset(2)] public short Attributes;
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct COLORREF
    {
        internal uint ColorDWORD;

        internal COLORREF(Color color)
        {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        internal COLORREF(uint r, uint g, uint b)
        {
            ColorDWORD = r + (g << 8) + (b << 16);
        }

        internal Color GetColor()
        {
            return Color.FromArgb((int)(0x000000FFU & ColorDWORD),
               (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
        }

        internal void SetColor(Color color)
        {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    internal struct CONSOLE_SCREEN_BUFFER_INFO_EX
    {
        internal int cbSize;
        internal SmallCoord dwSize;
        internal SmallCoord dwCursorPosition;
        internal ushort wAttributes;
        internal SmallRect srWindow;
        internal SmallCoord dwMaximumWindowSize;
        internal ushort wPopupAttributes;
        internal bool bFullscreenSupported;
        internal COLORREF black;
        internal COLORREF darkBlue;
        internal COLORREF darkGreen;
        internal COLORREF darkCyan;
        internal COLORREF darkRed;
        internal COLORREF darkMagenta;
        internal COLORREF darkYellow;
        internal COLORREF gray;
        internal COLORREF darkGray;
        internal COLORREF blue;
        internal COLORREF green;
        internal COLORREF cyan;
        internal COLORREF red;
        internal COLORREF magenta;
        internal COLORREF yellow;
        internal COLORREF white;
    }
}
