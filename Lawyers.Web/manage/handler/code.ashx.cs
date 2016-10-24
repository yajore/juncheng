using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Lawyers.Web.manage.handler
{
    /// <summary>
    /// code 的摘要说明
    /// </summary>
    public class code : IHttpHandler, IRequiresSessionState
    {
        static Color[] colorstring = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.DarkCyan, Color.Purple };
        static Color[] colorline = { Color.LightBlue, Color.LightCoral, Color.LightCyan, Color.LightGoldenrodYellow, Color.LightGray, Color.LightGreen, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.LightSkyBlue, Color.LightYellow };
        public void ProcessRequest(HttpContext context)
        {

            Random rand = new Random();
            string code = new Random().Next(1000, 9999).ToString();

            var image = new Bitmap(110, 30);
            var g = Graphics.FromImage(image);
            g.Clear(Color.White);
            var font = new Font("Arial", 18, FontStyle.Bold);


            for (int i = 0; i < 30; i++)
            {
                var blackpen = new Pen(colorline[rand.Next(colorline.Length - 1)], 2);
                int x1 = rand.Next(image.Width);
                int y1 = rand.Next(image.Height);
                int x2 = rand.Next(image.Width);
                int y2 = rand.Next(image.Height);
                g.DrawLine(blackpen, x1, y1, x2, y2);
            }
            for (int i = 0; i < code.Length; i++)
            {
                var brush = new SolidBrush(colorstring[rand.Next(colorstring.Length - 1)]);
                int strleft = rand.Next(3);
                int strtop = rand.Next(3);
                g.DrawString(code[i].ToString(CultureInfo.InvariantCulture), font, brush, 1 + strleft + 25 * i, strtop);
            }
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            g.Dispose();
            image.Dispose();

            context.Session["jiucheng_code"] = code;
            context.Response.ContentType = "image/png";
            context.Response.BinaryWrite(ms.ToArray());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}