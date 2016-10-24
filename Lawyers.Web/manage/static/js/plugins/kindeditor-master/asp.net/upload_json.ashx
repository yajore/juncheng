<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Globalization;
using System.Configuration;
using System.Linq;
using Aliyun.OSS;
using Aliyun.OSS.Common;
using Mango.Utilities;

public class Upload : IHttpHandler
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
            showError("请选择文件。");
        }



        String fileName = imgFile.FileName;
        String fileExt = Path.GetExtension(fileName).ToLower();

        if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
        {
            showError("上传文件大小超过(2MB)限制。");
        }

        if (String.IsNullOrEmpty(fileExt) || extTable.Contains(fileExt) == false)
        {
            showError("上传文件扩展名是不允许的扩展名。\n只允许" + String.Join(",", extTable) + "格式。");
        }

        Hashtable hash = new Hashtable();
        try
        {

            string relationPath = "/upload/activity/images/";
            string file = DateTime.Now.ToString("yyMMddHHmmssfff") + ".jpg";

            if (!Directory.Exists(basePath + relationPath))
            {
                Directory.CreateDirectory(basePath + relationPath);
            }
            string fullFile = basePath + relationPath + file;
            imgFile.SaveAs(fullFile);


            string key = "activity/" + file;
            PutObject(key, fullFile);

            hash["error"] = 0;
            hash["url"] = cdnurl + "/" + key;
        }
        catch (Exception ex)
        {
            hash["error"] = -10;
            hash["url"] = ex.Message;
            Logger.WriteToFile("活动图片上传", ex);
        }

        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(JsonHelper.ReBuilder(hash));
        context.Response.End();
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

    private void showError(string message)
    {
        Hashtable hash = new Hashtable();
        hash["error"] = 1;
        hash["message"] = message;
        context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
        context.Response.Write(Mango.Utilities.JsonHelper.ReBuilder(hash));
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
}
