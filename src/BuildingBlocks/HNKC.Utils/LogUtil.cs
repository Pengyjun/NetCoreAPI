using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HNKC.Utils
{
    /// <summary>
    /// 静态日志，写入文件
    /// </summary>
    public static class LogUtil
    {
        public static void Debug(string message) => WriteLog(message, LoggerLevel.Debug);
        public static void Info(string message) => WriteLog(message, LoggerLevel.Info);
        public static void Warning(string message) => WriteLog(message, LoggerLevel.Warning);
        public static void Error(string message) => WriteLog(message, LoggerLevel.Error);
        private static void WriteLog(string message, LoggerLevel loggerLevel)
        {
            string path = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = AppDomain.CurrentDomain.BaseDirectory + $"Log\\{Enum.GetName(typeof(LoggerLevel), loggerLevel)}\\";
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + $"Log/{Enum.GetName(typeof(LoggerLevel), loggerLevel)}/";
            }
            var nowTime = DateTime.Now;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileFullPath = path + nowTime.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append($"Level:  {Enum.GetName(typeof(LoggerLevel), loggerLevel)}\r\n");
            str.Append("Time:    " + nowTime.ToString("yyyy-MM-dd hh:mm:ss") + "\r\n");
            str.Append("Message: " + message + "\r\n");
            str.Append("-----------------------------------------------------------\r\n\r\n");
            StreamWriter sw;
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }

    public enum LoggerLevel
    {
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
    }
}
