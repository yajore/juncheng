using Aliyun.OSS;
using Aliyun.OSS.Common;
using Lawyers.BizEntities;
using Lawyers.Utilities;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;

namespace Lawyers.Web.manage.handler
{
    /// <summary>
    /// fileupload 的摘要说明
    /// </summary>
    public class fileupload : IHttpHandler
    {
        static string basePath = HttpContext.Current.Server.MapPath("~/");
        private static string accessId = ConfigurationManager.AppSettings["aliyun_accessId"];// "3TK2c0EbNHi8UDPN";
        private static string accessKey = ConfigurationManager.AppSettings["aliyun_accessKey"];// "capu2wl19W0I1k9WAzebwdfBuHXsUb";
        private static string endpoint = ConfigurationManager.AppSettings["aliyun_endpoint"];// "http://oss-cn-hangzhou.aliyuncs.com";
        private static string imagetemp = ConfigurationManager.AppSettings["aliyun_imageTemp"];//上传图片临时文件夹
        private static string cdnurl = ConfigurationManager.AppSettings["aliyun_cdnUrl"];//http://cdn.daneigou.com/
        private static string bucketName = ConfigurationManager.AppSettings["aliyun_bucketName"];// "yachun";
        private static OssClient client = new OssClient(endpoint, accessId, accessKey);
        public void ProcessRequest(HttpContext context)
        {
            var imgdata = context.Request["imgdata"];
            var id = context.Request["id"];
            var type = context.Request["type"] ?? "user";
            if (String.IsNullOrEmpty(imgdata) == false)
            {
                context.Response.Write(GetFilePath(imgdata, id, type));
            }
            context.Response.End();
        }

        private string GetFilePath(string base64, string uid, string type)
        {
            var result = new OperationResult();
            try
            {
                if (base64.Contains(","))
                {
                    base64 = base64.Split(',')[1];
                }
                byte[] bytes = Convert.FromBase64String(base64);
                var memStream = new MemoryStream(bytes);
                var bmp = new Bitmap(memStream);
                string relationPath = "/upload/images/" + type + "/";
                string file = uid + "_" + DateTime.Now.ToString("yyMMddHHmmss") + ".jpg";
                if (!Directory.Exists(basePath + relationPath))
                {
                    Directory.CreateDirectory(basePath + relationPath);
                }
                string fullFile = basePath + relationPath + file;
                bmp.Save(fullFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                bmp.Dispose();
                memStream.Dispose();

                //PutObject("user/face/" + file, fullFile);

                //return cdnurl + uid + "_cie_cert" + "/" + file;

                //return domain + "user/face/" + file;
                string key = type + "/" + file;
                PutObject(key, fullFile);
                result.Fields = cdnurl + "/" + key;
                result.ErrCode = 0;
            }
            catch (Exception ex)
            {
                result.ErrCode = -1;
                result.Message = ex.Message;

                Logger.WriteToFile("GetFilePath", ex);
            }
            return JsonHelper.ReBuilder(result);
        }

        private string PutObject(string key, string fileToUpload)
        {
            try
            {

                PutObjectResult result = client.PutObject(bucketName, key, fileToUpload);
                string str = result.ETag;
                return str;
            }
            catch (OssException ex)
            {
                throw new Exception(ex.Message);
            }
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