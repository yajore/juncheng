using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Lawyers.Utilities
{
    public class AsynLogHelper : IDisposable
    {
        /// <summary>
        /// 模块完整路径
        /// </summary>
        private readonly string CONFIG_FILE = Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// 日志路径
        /// </summary>
        private string LogPath { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        private byte[] LogContent { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="logPath"></param>
        public AsynLogHelper(string logPath)
        {
            this.LogPath = logPath;
            this.CreateLogPath();
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="logPath"></param>
        public AsynLogHelper()
        {
            this.CreateLogPath();
        }

        public void WriteEntry(string formTarget, string details)
        {
            this.FormatContent(formTarget, details);
            this.WriterLog();
        }

        /// <summary>
        /// 自定义
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="append"></param>
        public void WriteFile(string file, string content, bool append)
        {
            try
            {
                file = this.LogPath + file;

                if (!File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);

                    fs.Close();
                }

                using (StreamWriter writer = new StreamWriter(file, append, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(content);

                    writer.Flush();

                    writer.Close();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadToFile(string file)
        {
            file = this.LogPath + file;

            var result = String.Empty;

            if (File.Exists(file) == false)
            {
                return result;
            }
         

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string line = string.Empty;

                using (var sr = new StreamReader(fs, System.Text.Encoding.UTF8))
                {

                    result = sr.ReadToEnd();

                    sr.Close();
                }
                fs.Close();
            }

            return result;
        }

        private void WriterLog()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            FileStream fileStream = null;

            if (!this.InitFileStream(ref fileStream))
            {
                return;
            }

            fileStream.BeginWrite(this.LogContent, 0, this.LogContent.Length, new AsyncCallback(this.EndWriteCallback), new State(fileStream, manualResetEvent));

            manualResetEvent.WaitOne(6000, false);

            fileStream.Close();

            fileStream = null;
        }

        /// <summary>
        /// 初始化文件流
        /// </summary>
        /// <param name="fStream"></param>
        /// <returns></returns>
        private bool InitFileStream(ref FileStream fStream)
        {
            string path = LogPath + OperateFile.DEFULT_NAME;

            CreateLogPath();

            int bufferSize = 1024;

            try
            {
                if (File.Exists(path))
                {
                    fStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, bufferSize, FileOptions.Asynchronous);
                }
                else
                {
                    fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, bufferSize, FileOptions.Asynchronous);
                }
            }
            catch (Exception ex)
            {
                OperateFile.ShowException(ex, new StackTrace(true));

                return false;
            }

            return fStream.CanWrite;
        }

        /// <summary>
        /// 完成写入
        /// </summary>
        /// <param name="asyncResult"></param>
        private void EndWriteCallback(IAsyncResult asyncResult)
        {
            State state = (State)asyncResult.AsyncState;
            FileStream fStream = state.FStream;
            fStream.EndWrite(asyncResult);
            state.ManualEvent.Set();
        }

        /// <summary>
        /// 创建日志路径
        /// </summary>
        private void CreateLogPath()
        {
            this.LogPath = OperateFile.CreateLogDirectory(this.LogPath);
        }

        /// <summary>
        /// 格式化内容
        /// </summary>
        /// <param name="formTarget"></param>
        /// <param name="details"></param>
        private void FormatContent(string formTarget, string details)
        {
            string text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss", CultureInfo.InvariantCulture);

            string s = string.Format(CultureInfo.InvariantCulture, "\r\n{0}  [{1}]\r\n{2}\r\n ", new object[] { text, formTarget, details });

            this.LogContent = Encoding.UTF8.GetBytes(s);
        }

        /// <summary>
        /// 释放资源，回收垃圾
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Trace.Close();
                GC.SuppressFinalize(this);
            }
        }
    }

    internal class State
    {
        private FileStream fStream;

        private ManualResetEvent manualEvent;

        public FileStream FStream
        {
            get
            {
                return this.fStream;
            }
        }

        public ManualResetEvent ManualEvent
        {
            get
            {
                return this.manualEvent;
            }
        }

        public State(FileStream fStream, ManualResetEvent manualEvent)
        {
            this.fStream = fStream;
            this.manualEvent = manualEvent;
        }
    }

    internal class OperateFile
    {
        private const string SEARCH_PATTERN = "*.log";
        private const long FILE_BYTE = 5249000L;
        public static readonly string DEFULT_PATH = string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
        {
            AppDomain.CurrentDomain.BaseDirectory,
            "\\Log"
        });

        public static string DEFULT_NAME
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}.log", new object[]
        {
            DateTime.Now.ToString("yyyy-MM-dd HH", CultureInfo.InvariantCulture)
        });
            }
        }

        /// <summary>
        /// 创建日志路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected internal static string CreateLogDirectory(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            return path;
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="stackTrace"></param>
        protected internal static void ShowException(Exception ex, StackTrace stackTrace)
        {
            string message = string.Format(CultureInfo.InvariantCulture, "Error information:{0}\r\nError method:{1}\r\nException information{2}\r\nError linenumber{3}", new object[]
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException,
                stackTrace.GetFrame(0).GetFileLineNumber()
            });
            string category = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", new object[]
            {
                stackTrace.GetFrame(0).GetFileName(),
                stackTrace.GetFrame(0).GetMethod().Name
            });
            Trace.WriteLine(string.Empty);
            Trace.WriteLine(message, category);
            Trace.WriteLine(string.Empty);
            Trace.Flush();
        }
    }
}
