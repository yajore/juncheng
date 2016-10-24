using System;
using System.Text;

namespace Lawyers.Utilities
{
    public class Logger
    {
        private static string path = System.AppDomain.CurrentDomain.BaseDirectory;


        public static void WriteToFile(string methodName, string message)
        {
            AsynLogHelper log = new AsynLogHelper(path + "log/");
            log.WriteEntry(methodName, message);
        }
        public static void WriteToFile(string methodName, string module, string message)
        {
            AsynLogHelper log = new AsynLogHelper(path + "log/" + module + "/");
            log.WriteEntry(methodName, message);
        }
        public static void WriteToFile(string methodName, Exception exception)
        {
            string ex = GetExceptionDetail(methodName, exception);
            AsynLogHelper log = new AsynLogHelper(path + "log/");
            log.WriteEntry(methodName, ex);
        }
        public static void WriteToFile(string methodName, string folder, Exception exception)
        {
            string ex = GetExceptionDetail(methodName, exception);
            AsynLogHelper log = new AsynLogHelper(path + "log/" + folder + "/");
            log.WriteEntry(methodName, ex);
        }

        public static void WriteException(string methodName, Exception exception, object requestParam)
        {
            string ex = GetExceptionDetail(methodName, exception, JsonHelper.ReBuilder(requestParam));
            AsynLogHelper log = new AsynLogHelper(path + "log/exception/");
            log.WriteEntry(methodName, ex);
        }

        //获取详细异常信息
        private static string GetExceptionDetail(string method, System.Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\r\n" + "系统时间：" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
            sb.AppendLine("---------------------日志记录开始------------------------");
            sb.Append("日志说明：" + method);
            sb.AppendFormat("Exception Type:{0}\r\n", ex.GetType().Name);
            if (ex.Message != null)
            {
                sb.AppendFormat("Exception Message:{0}\r\n", ex.Message);
            }
            if (ex.Source != null)
            {
                sb.AppendFormat("Exception Source:{0}\r\n", ex.Source);
            }
            if (ex.TargetSite != null)
            {
                sb.AppendFormat("Module Name: {0}\r\n", ex.TargetSite.Module.FullyQualifiedName);
            }
            if (ex.StackTrace != null)
            {
                sb.AppendFormat("Exception Trace:\r\n{0}\r\n", ex.StackTrace);
            }
            sb.AppendLine("---------------------日志记录结束------------------------");
            return sb.ToString();
        }

        private static string GetExceptionDetail(string method, System.Exception ex, string requestParam)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\r\n" + "系统时间：" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"));
            sb.AppendLine("---------------------日志记录开始------------------------");
            sb.AppendFormat("请求参数:{0}\r\n", requestParam + "\r\n");
            sb.Append("日志说明：" + method);
            sb.AppendFormat("Exception Type:{0}\r\n", ex.GetType().Name);
            if (ex.Message != null)
            {
                sb.AppendFormat("Exception Message:{0}\r\n", ex.Message);
            }
            if (ex.Source != null)
            {
                sb.AppendFormat("Exception Source:{0}\r\n", ex.Source);
            }
            if (ex.TargetSite != null)
            {
                sb.AppendFormat("Module Name: {0}\r\n", ex.TargetSite.Module.FullyQualifiedName);
            }
            if (ex.StackTrace != null)
            {
                sb.AppendFormat("Exception Trace:\r\n{0}\r\n", ex.StackTrace);
            }


            sb.AppendLine("---------------------日志记录结束------------------------");
            return sb.ToString();
        }

    }
}
