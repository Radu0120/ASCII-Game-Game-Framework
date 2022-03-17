using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCMandatory1
{
    public static class Color
    {
        public static string Foreground(int red, int green, int blue)
        {
            return "\x1b[38;2;" + red + ";" + green + ";" + blue + "m";
        }
        public static string Foreground(int[]rgb)
        {
            return "\x1b[38;2;" + rgb[0] + ";" + rgb[1] + ";" + rgb[2] + "m";
        }
        public static string Background(int red, int green, int blue)
        {
            return "\x1b[48;2;" + red + ";" + green + ";" + blue + "m";
        }
        public static string Background(int[] rgb)
        {
            return "\x1b[48;2;" + rgb[0] + ";" + rgb[1] + ";" + rgb[2] + "m";
        }
        public static int[] Red
        {
            get { int[] rgb = new int[] { 255, 0, 0 }; return rgb; }
        }
        public static int[] Green
        {
            get { int[] rgb = new int[] { 0, 255, 0 }; return rgb; }
        }
        public static int[] Blue
        {
            get { int[] rgb = new int[] { 0, 0, 255 }; return rgb; }
        }
        public static int[] Yellow
        {
            get { int[] rgb = new int[] { 255, 255, 0 }; return rgb; }
        }
        public static int[] Orange
        {
            get { int[] rgb = new int[] { 255, 128, 0 }; return rgb; }
        }
        public static int[] LawnGreen
        {
            get { int[] rgb = new int[] { 128, 255, 0 }; return rgb; }
        }
        public static int[] Lime
        {
            get { int[] rgb = new int[] { 0, 255, 128 }; return rgb; }
        }
        public static int[] Cyan
        {
            get { int[] rgb = new int[] { 0, 255, 255 }; return rgb; }
        }
        public static int[] LightBlue
        {
            get { int[] rgb = new int[] { 0, 128, 255 }; return rgb; }
        }
        public static int[] Violet
        {
            get { int[] rgb = new int[] { 128, 0, 255 }; return rgb; }
        }
        public static int[] Magenta
        {
            get { int[] rgb = new int[] { 255, 0, 255 }; return rgb; }
        }
        public static int[] White
        {
            get { int[] rgb = new int[] { 255, 255, 255 }; return rgb; }
        }
        public static int[] Pink
        {
            get { int[] rgb = new int[] { 255, 0, 128 }; return rgb; }
        }
        public static int[] Gray
        {
            get { int[] rgb = new int[] { 128, 128, 128 }; return rgb; }
        }
        public static int[] Black
        {
            get { int[] rgb = new int[] { 12, 12, 12 }; return rgb; }
        }
    }
}
