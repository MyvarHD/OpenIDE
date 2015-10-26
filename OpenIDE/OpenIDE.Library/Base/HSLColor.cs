﻿namespace OpenIDE.Library
{
    using System;
    using System.Drawing;

    public class HSLColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private const double scale = 240.0;
        private double hue = 1.0;
        private double luminosity = 1.0;
        private double saturation = 1.0;

        public HSLColor()
        {
        }

        public HSLColor(Color color)
        {
            this.SetRGB(color.R, color.G, color.B);
        }

        public HSLColor(int red, int green, int blue)
        {
            this.SetRGB(red, green, blue);
        }

        public HSLColor(double hue, double saturation, double luminosity)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Luminosity = luminosity;
        }

        public double Hue
        {
            get { return this.hue*scale; }
            set { this.hue = this.CheckRange(value/scale); }
        }

        public double Saturation
        {
            get { return this.saturation*scale; }
            set { this.saturation = this.CheckRange(value/scale); }
        }

        public double Luminosity
        {
            get { return this.luminosity*scale; }
            set { this.luminosity = this.CheckRange(value/scale); }
        }

        private double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", this.Hue, this.Saturation, this.Luminosity);
        }

        public string ToRGBString()
        {
            Color color = this;
            return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
        }

        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = Color.FromArgb(red, green, blue);
            this.hue = hslColor.hue;
            this.saturation = hslColor.saturation;
            this.luminosity = hslColor.luminosity;
        }

        #region Casts to/from System.Drawing.Color

        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0*hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0/3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0/3.0);
                }
            }
            return Color.FromArgb((int) (255*r), (int) (255*g), (int) (255*b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0/6.0)
                return temp1 + (temp2 - temp1)*6.0*temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0/3.0)
                return temp1 + ((temp2 - temp1)*((2.0/3.0) - temp3)*6.0);
            else
                return temp1;
        }

        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }

        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5) //<=??
                temp2 = hslColor.luminosity*(1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity*hslColor.saturation);
            return temp2;
        }

        public static implicit operator HSLColor(Color color)
        {
            var hslColor = new HSLColor();
            hslColor.hue = color.GetHue()/360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            return hslColor;
        }

        #endregion
    }
}