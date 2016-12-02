using Lawyers.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Lawyers.Web
{
    /// <summary>
    /// qrcode 的摘要说明
    /// </summary>
    public class qrcode : IHttpHandler
    {
        static string baseRoot = HttpContext.Current.Server.MapPath("~/");
        static int initialWidth = 330, initialHeight = 330;
        public void ProcessRequest(HttpContext context)
        {

            var nickname = context.Request["nick"];
            var text = context.Request["data"];
            var product = context.Request["product"];
            var price = context.Request["price"];
            if (String.IsNullOrEmpty(nickname) ||
                String.IsNullOrEmpty(product) ||
                String.IsNullOrEmpty(text) ||
                String.IsNullOrEmpty(price))
            {
                return;
            }
            nickname = HttpUtility.UrlDecode(nickname);
            text = HttpUtility.UrlDecode(text);
            product = HttpUtility.UrlDecode(product);
            var midword = "为您推荐了";

            Bitmap theBitmap = new Bitmap(initialWidth, initialHeight);
            Graphics theGraphics = Graphics.FromImage(theBitmap);
            //呈现质量
            theGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //背景色
            theGraphics.Clear(Color.FromArgb(255, 255, 255));
            //边框
            //theGraphics.DrawRectangle(new Pen(Color.FromArgb(61, 173, 47), 1), 1, 1, initialWidth - 2,
            //    initialHeight - 2);
            //圆弧边框
            Pen p = new Pen(Color.FromArgb(61, 173, 47), 1);
            Rectangle rect = new Rectangle(1, 1, initialWidth - 2, initialHeight - 2);
            using (GraphicsPath path = new ImageUtility().CreateRoundedRectanglePath(rect, 7))
            {
                theGraphics.DrawPath(p, path);
                theGraphics.FillPath(new SolidBrush(Color.White), path);
            }


            string FontType = "微软雅黑";
            Font theFont = new Font(FontType, 18f, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);

            //准备工作。定义画刷颜色

            Brush greenBrush = new SolidBrush(Color.FromArgb(61, 173, 47)); //填充的颜色
            Brush greyBrush = new SolidBrush(Color.FromArgb(144, 144, 144)); //填充的颜色
            //var img = Bitmap.FromFile(baseRoot + "//images//slogo.jpg");//图片地址
            var imgFile = baseRoot + "//images//slogo.jpg";


            var qrcode = QRCodeHelper.CreateQRCodeWithLogo(text, imgFile);

            //var thumbnail = GetThumbnail(img, 35, 35);
            var imgX = (initialWidth - qrcode.Width) / 2;
            var imgY = (initialHeight - qrcode.Height) / 2;
            theGraphics.DrawImage(qrcode, new System.Drawing.Rectangle(imgX, imgY, qrcode.Width, qrcode.Height),
                                new System.Drawing.Rectangle(0, 0, qrcode.Width, qrcode.Height),
                                System.Drawing.GraphicsUnit.Pixel);


            nickname = nickname.Length > 4 ? nickname.Substring(0, 4) : nickname;
            midword = "为您推荐了";
            product = product.Length > 4 ? product.Substring(0, 5) : product;
            price = "￥" + price;
            var tips = "（长按识别二维码参团购买）";

            var start = 40;
            if (nickname.Length == 1)
            {
                start = 75;
            }
            else if (nickname.Length == 2)
            {
                start = 60;
            }
            else if (nickname.Length == 3)
            {
                start = 50;
            }
            else if (nickname.Length == 4)
            {
                start = 30;
            }
            theGraphics.DrawString(nickname, theFont, greenBrush, start, 15);
            theGraphics.DrawString(midword, theFont, greyBrush, 115, 15);
            theGraphics.DrawString(product, theFont, greenBrush, 220, 15);

            theGraphics.DrawString(price, theFont, greyBrush, 122, 40);
            theGraphics.DrawString(tips, theFont, greyBrush, 47, 275);

            MemoryStream ms = new MemoryStream();
            theBitmap.Save(ms, ImageFormat.Png);
            //context.Session["code"] = code;
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(ms.ToArray());
            theGraphics.Dispose();
            theBitmap.Dispose();
            ms.Dispose();
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