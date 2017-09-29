using System;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace foosballv2s
{
    public class BallImage
    {
        private Image<Hsv, Byte> image;

        public BallImage(String file)
        {
            this.image = new Image<Hsv, Byte>(file);
        }

        public Hsv getColor()
        {
            Bitmap bitmap = this.image.ToBitmap();

            Color colorAvg;
            Color color1 = bitmap.GetPixel(bitmap.Width / 2, bitmap.Height / 2);            //spalva is centro
            Color color2 = bitmap.GetPixel(bitmap.Width / 2 + 10, bitmap.Height / 2 + 10);  //spalva aplink centra
            Color color3 = bitmap.GetPixel(bitmap.Width / 2 - 10, bitmap.Height / 2 - 10);
            Color color4 = bitmap.GetPixel(bitmap.Width / 2 + 10, bitmap.Height / 2 - 10);
            Color color5 = bitmap.GetPixel(bitmap.Width / 2 - 10, bitmap.Height / 2 + 10);

            int r = (color1.R + color2.R + color3.R + color4.R + color5.R) / 5; //gaunama vidutinė spalva
            int g = (color1.G + color2.G + color3.G + color4.G + color5.G) / 5;
            int b = (color1.B + color2.B + color3.B + color4.B + color5.B) / 5;

            colorAvg = Color.FromArgb(r, g, b);

            Hsv hsv = new Hsv(colorAvg.GetHue() / 2,    //color.GetHue() [0;360] -> Hue [0;180]
                colorAvg.GetSaturation() * 255,         //color.GetSaturation() [0;1] -> Saturation [0;255]
                colorAvg.GetBrightness() * 255);        //color.GetBrightness() [0;1] -> Brightness [0;255]

            /*Console.WriteLine("RGB:     " + color1.R + " " + color1.G + " " + color1.B);
            Console.WriteLine("RGB vid: " + r + " " + g + " " + b);
            Console.WriteLine("HSVvid [0;180], [0;255], [0;255]: "
                + hsv.Hue + " "
                + hsv.Satuation + " "
                + hsv.Value);*/

            return hsv;
        }
    }
}
