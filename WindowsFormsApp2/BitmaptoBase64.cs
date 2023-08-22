using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class BitmaptoBase64
    {
        public static string BitmapToBase64(Bitmap bitmap)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                // Chuyển đổi Bitmap thành byte[]
                using (Bitmap bmp = new Bitmap(bitmap))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                byte[] imageBytes = ms.ToArray();

                // Chuyển đổi byte[] thành Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}
