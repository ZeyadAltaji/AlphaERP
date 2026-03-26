using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace AlphaERP.Controllers
{
    public class Captcha
    {
        private Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
        private static readonly Random Randomizer = new Random(DateTime.Now.Second);
        public string Text { get; set; }
        public byte[] ImageAsByteArray { get; set; }

        public Captcha()
        {
            Text = GetRandomText();
            ImageAsByteArray = CreateCaptcha(Text);
        }
        private static string GetRandomText()
        {
            string text = "";
            const string chars = "0123456789";
            for (int i = 0; i < 6; i++)
            {
                text += chars.Substring(Randomizer.Next(0, chars.Length), 1);
            }

            return text;
        }

        private static byte[] CreateCaptcha(string text)
        {
            Color bg = Color.FromArgb(232, 240, 254);
            List<Brush> _brushes = new List<Brush>
            {
                Brushes.Black,
                Brushes.Red,
                Brushes.Green,
                Brushes.DarkMagenta,
                Brushes.DarkViolet,
                Brushes.DarkTurquoise,
                Brushes.DarkSlateGray,
                Brushes.DarkSeaGreen,
                Brushes.DarkGoldenrod,
                Brushes.Brown,
                Brushes.Coral,
                Brushes.Orange,
                Brushes.DarkOrange,
                Brushes.OrangeRed
            };
            byte[] byteArray = null;
            Font[] fonts = {
            new Font("cursive", 19,FontStyle.Italic ),
            new Font("fantasy", 13,FontStyle.Italic ),
            new Font("monospace", 14,FontStyle.Italic |  FontStyle.Bold),
            new Font("inherit", 12,FontStyle.Italic |  FontStyle.Bold),
            new Font("Arial", 8,FontStyle.Italic |  FontStyle.Bold),
            new Font("Courier New", 18,FontStyle.Italic |  FontStyle.Bold),
            new Font("Calibri", 25,FontStyle.Italic |  FontStyle.Bold),
            new Font("Tahoma", 20, FontStyle.Italic | FontStyle.Bold) };
            using (Bitmap bmp = new Bitmap(107, 34))
            {
                using (Graphics graphic = Graphics.FromImage(bmp))
                {
                    Random rnd = new Random();
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    using (HatchBrush hb = new HatchBrush(HatchStyle.DarkUpwardDiagonal, bg, bg))
                    {
                        graphic.FillRectangle(hb, 0, 0, bmp.Width, bmp.Height);
                    }

                    for (int i = 0; i < text.Length; i++)
                    {
                        Brush result = Brushes.Transparent;


                        result = _brushes[rnd.Next(_brushes.Count)];

                        PointF point = new PointF((i * 16), 18);
                        graphic.DrawString(text.Substring(i, 1), fonts[Randomizer.Next(0, 4)], result, point, new StringFormat { LineAlignment = StringAlignment.Center });
                    }
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(stream, ImageFormat.Png);
                    byteArray = stream.ToArray();
                }
            }
            foreach (Font font in fonts)
            {
                font.Dispose();
            }

            return byteArray;
        }
    }
}