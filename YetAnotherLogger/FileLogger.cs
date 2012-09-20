using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YAL
{
    public class FileLogger : BaseLogger
    {
        internal FileLogger() { }

        /// <summary>
        /// Logs a new line of text to a text file
        /// </summary>
        /// <param name="type">The type of the error to log</param>
        /// <param name="text">The error message to log.</param>
        public override void Log(LoggType type, string text)
        {
            LoggInfo loggInfo = new LoggInfo(text, type);
            Log(loggInfo);
        }

        /// <summary>
        /// Logs an error to a text file
        /// </summary>
        /// <param name="loggInfo">The <see cref="LoggInfo"/> to log from.</param>
        public override void Log(LoggInfo loggInfo)
        {
            StringBuilder loggBuilder = new StringBuilder(loggInfo.Date.ToString());
            loggBuilder.AppendFormat(" - DEF: {0}", loggInfo.Type.ToString());
            if (!String.IsNullOrWhiteSpace(loggInfo.Text))
            {
                loggBuilder.AppendFormat(" - TEXT: {0}", loggInfo.Text);
            }
            if (loggInfo.Exception != null)
            {
                loggBuilder.AppendFormat(" - EXTYPE {0}", loggInfo.Exception.GetType());
                loggBuilder.AppendFormat(" - EXCEPTION: {0} - STACKTRACE: {1}", loggInfo.Exception.Message, loggInfo.Exception.StackTrace);
            }

            string filePath = Path.GetFullPath(Path.Combine(BaseDirectory, AppName, FileName));
            CreateDirIfNotExist();
            using (StreamWriter log_writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                log_writer.WriteLine(loggBuilder.ToString());
            }
        }
    }
}
