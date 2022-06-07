using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Pixel
    {
        public const short Black = 0b0000_0000;
        public const short B_Blue = 0b0001_0000;
        public const short B_Green = 0b0010_0000;
        public const short B_Red = 0b0100_0000;
        public const short B_Intensity = 0b1000_0000;

        public const short F_Blue = 0x0001;
        public const short F_Green = 0x0002;
        public const short F_Red = 0x0004;
        public const short F_Intensity = 0x0008;


        public short B_Color;
        public short F_Color;
        public char Char;

        public static short ConvertColor(int[] rgb, bool background)
        {
            short color = 0b0000_0000;
            if(rgb[0] > 12)
            {
                if (background)
                {
                    if (rgb[0] > 128)
                    {
                        color |= B_Intensity;
                    }
                    color |= B_Red;
                }
                else
                {
                    if (rgb[0] > 128)
                    {
                        color |= F_Intensity;
                    }
                    color |= F_Red;
                }
            }
            if (rgb[1] > 12)
            {
                if (background)
                {
                    if (rgb[1] > 128)
                    {
                        color |= B_Intensity;
                    }
                    color |= B_Green;
                }
                else
                {
                    if (rgb[1] > 128)
                    {
                        color |= F_Intensity;
                    }
                    color |= F_Green;
                }
            }
            if (rgb[2] > 12)
            {
                if (background)
                {
                    if (rgb[2] > 128)
                    {
                        color |= B_Intensity;
                    }
                    color |= B_Blue;
                }
                else
                {
                    if (rgb[2] > 128)
                    {
                        color |= F_Intensity;
                    }
                    color |= F_Blue;
                }
            }
            return color;
        }

        //public static short ConvertColor(int[] rgb, bool background)
        //{
        //    short color = 0b0000_0000;
        //    if (rgb.SequenceEqual(Color.Red))
        //    {
        //        if (background) { color = B_Red; }
        //        else color = F_Red;
        //        return color;
        //    }
        //    if (rgb.SequenceEqual(Color.Green))
        //    {
        //        if (background) { color = B_Green; }
        //        else color = F_Green;
        //        return color;
        //    }
        //    if (rgb.SequenceEqual(Color.Blue))
        //    {
        //        if (background) { color = B_Blue; }
        //        else color = F_Blue;
        //        return color;
        //    }
        //    if (rgb.SequenceEqual(Color.Gray))
        //    {
        //        if (background) { color = Black | B_Intensity; }
        //        else color = Black | F_Intensity;
        //        return color;
        //    }

        //    if (background)
        //    {
        //        if (rgb[0] == 255)
        //        {
        //            color |= F_Red << 4;
        //        }
        //        if (rgb[1] == 255)
        //        {
        //            color |= F_Green << 4;
        //        }
        //        if (rgb[2] == 255)
        //        {
        //            color |= F_Blue << 4;
        //        }
        //    }
        //    else
        //    {
        //        if (rgb[0] == 255)
        //        {
        //            color |= F_Red;
        //        }
        //        if (rgb[1] == 255)
        //        {
        //            color |= F_Green;
        //        }
        //        if (rgb[2] == 255)
        //        {
        //            color |= F_Blue;
        //        }
        //    }
        //    return color;
        //}

    }
}
