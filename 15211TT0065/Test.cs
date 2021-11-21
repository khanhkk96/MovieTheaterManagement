using System;
using System.Drawing;
using System.IO;

namespace _15211TT0065
{
    public static class Test
    {
        //kiểm tra chuỗi là số (int) hay k?
        public static bool IsInterger(this string s)
        {
            bool flag = false;
            int i = 0;
            flag = int.TryParse(s, out i);
            return flag;
        }

        //kiểm tra chuỗi là số (float) hay k?
        public static bool IsFloat(this string s)
        {
            bool flag = false;
            float i = 0;
            flag = float.TryParse(s, out i);
            return flag;
        }

        //kiểm tra chuỗi là số (double) hay k?
        public static bool IsDouble(this string s)
        {
            bool flag = false;
            double i = 0;
            flag = double.TryParse(s, out i);
            return flag;
        }

        //kiểm tra chuỗi có chứa số hay k?
        public static bool IsString(this string s)
        {
            bool flag = true;
            foreach (char c in s.ToCharArray())
            {
                if (Char.IsDigit(c))
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        public static byte[] ImageToByteArray(this System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static Image ResizeImage(this Image actualImage, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                graphics.DrawImage(actualImage, 0, 0, width, height);

            return (Image)bitmap;
        }
    }
}
