using System;
using System.IO;
using System.Reflection;

namespace Tools
{
    public delegate void LogDelegate(string message);
    public class Log
    {
        public static LogDelegate LogEvent;
        static string _path;
        public static string path
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_path))
                {
                    _path = ConfigHelper.GetConfigString("log:url");
                    if (string.IsNullOrEmpty(_path) || !Directory.Exists(_path))
                    {
                        _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    }
                }
                return _path;
            }
        }
        static string GetFilePath()
        {
            string _path = Path.Combine(path, "logs", DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            string _dir = Path.GetDirectoryName(_path);
            if (!Directory.Exists(_dir))
                Directory.CreateDirectory(_dir);
            return _path;
        }
        static object lockobj = new object();
        static StreamWriter streamwriter;
        public static void Add(string message)
        {
            LogEvent?.Invoke(message);
            lock (lockobj)
            {
                string _message = DateTime.Now.ToString("★yyyy-MM-dd HH;mm:ss.fff ") + message;
                using (streamwriter = new StreamWriter(GetFilePath(), true))
                {
                    streamwriter.WriteLine(_message);
                    streamwriter.Flush();
                    streamwriter.Close();
                    streamwriter.Dispose();
                }
            }
        }

        public static void Add(Exception ex)
        {
            if (ex.StackTrace != null)
            {
                Add(ex.StackTrace);
            }
        }
    }
}
