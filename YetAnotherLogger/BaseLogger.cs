using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YAL
{
    public abstract class BaseLogger
    {
        public static string AppName;
        public static string FileName;
        public static string BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static BaseLogger Create()
        {
            return Create("FileLogger");
        }

        public static BaseLogger Create(string loggerName)
        {
            if (string.Compare(loggerName, "FileLogger", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new FileLogger();
            }
            else if (string.Compare(loggerName, "XmlLogger", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return new XmlLogger();
            }

            throw new ArgumentException("The logger " + loggerName + " was not found.");
        }

        public abstract void Log(string type, string text);

        public abstract void Log(LoggInfo loggInfo);
    }
}
