using Aliyun.OSS;
using Aliyun.OSS.Common;
using Lawyers.BizEntities;
using Lawyers.BizProcess;
using Lawyers.Utilities;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Lawyers.Web.manage.handler
{
    /// <summary>
    /// imgupload 的摘要说明
    /// </summary>
    public class imgupload : IHttpHandler
    {
        private HttpContext context;
        //根目录
        static string basePath = HttpContext.Current.Server.MapPath("~/");
        //域名，用于拼接完整url
        static string domain = ConfigurationManager.AppSettings["domain"];

        string[] extTable = { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };

        private static string accessId = ConfigurationManager.AppSettings["aliyun_accessId"];
        private static string accessKey = ConfigurationManager.AppSettings["aliyun_accessKey"];
        private static string endpoint = ConfigurationManager.AppSettings["aliyun_endpoint"];
        private static string imagetemp = ConfigurationManager.AppSettings["aliyun_imageTemp"];//上传图片临时文件夹
        private static string cdnurl = ConfigurationManager.AppSettings["aliyun_cdnUrl"];
        private static string bucketName = ConfigurationManager.AppSettings["aliyun_bucketName"];
        private static OssClient client = new OssClient(endpoint, accessId, accessKey);
        public void ProcessRequest(HttpContext context)
        {
            //最大文件大小 2mb
            int maxSize = 2097152;
            this.context = context;
            //file控件，请将input file 控件的name设置为imgFile
            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                context.Response.Write("error|请选择文件");
                return;
            }



            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                context.Response.Write("error|上传文件大小超过(2MB)限制。");
                return;
            }

            if (String.IsNullOrEmpty(fileExt) || extTable.Contains(fileExt) == false)
            {
                context.Response.Write("上传文件扩展名是不允许的扩展名。\n只允许" + String.Join(",", extTable) + "格式。");
                return;
            }

            try
            {

                string relationPath = "/upload/article/images/";
                string file = fileName;

                if (!Directory.Exists(basePath + relationPath))
                {
                    Directory.CreateDirectory(basePath + relationPath);
                }
                string fullFile = basePath + relationPath + file;
                imgFile.SaveAs(fullFile);


                string key = "article/" + file;
                PutObject(key, fullFile);
                string fulllink = cdnurl + "/" + key;

                new MaterialBP().AddMaterialSync(new RequestOperation<MaterialData>()
                {
                    Header = new HeaderInfo(),
                    Body = new MaterialData()
                    {
                        Type = 1,
                        Url = fulllink
                    }
                });

                context.Response.Write(fulllink);


                return;

            }
            catch (Exception ex)
            {
                context.Response.Write("error|" + ex.Message);
                Logger.WriteToFile("活动图片上传", ex);
            }

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