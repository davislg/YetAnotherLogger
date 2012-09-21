using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

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
        /// <param name="innerException">The InnerException of this error</param>
        public override void Log(LoggType type, string text, Exception InnerException = null)
        {
            LoggInfo loggInfo = new LoggInfo(text, type, InnerException);
            Log(loggInfo);
        }

        /// <summary>
        /// Logs an error to a text file
        /// </summary>
        /// <param name="loggInfo">The <see cref="LoggInfo"/> to log from.</param>
        public override void Log(LoggInfo loggInfo)
        {
            StringBuilder loggBuilder = new StringBuilder(loggInfo.Date.ToString() + ";");
            loggBuilder.AppendFormat(" - DEF: {0};", loggInfo.Type.ToString());
            if (!String.IsNullOrWhiteSpace(loggInfo.Text))
            {
                loggBuilder.AppendFormat(" - TEXT: {0};", loggInfo.Text);
            }
            if (loggInfo.InnerException != null)
            {
                loggBuilder.AppendFormat(" - EXTYPE: {0};", loggInfo.InnerException.ExceptionType);
                loggBuilder.AppendFormat(" - EXCEPTION: {0} - STACKTRACE: {1}", loggInfo.InnerException.Message, loggInfo.InnerException.StackTrace);
            }

            string filePath = Path.GetFullPath(Path.Combine(BaseDirectory, AppName, FileName));
            CreateDirIfNotExist();
            AddToObservable(loggInfo);
            using (StreamWriter log_writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                log_writer.WriteLine(loggBuilder.ToString());
            }
        }

        protected override System.Collections.ObjectModel.ObservableCollection<LoggInfo> GetLogg()
        {
            ObservableCollection<LoggInfo> loggInfos = new ObservableCollection<LoggInfo>();

            Regex loggRegex = new Regex(@"^([0-9 \.\-\:]+); - DEF: (Error|Info|General|Warning);(?: - TEXT: (.+);)?(?: - EXTYPE: (.+); - EXCEPTION: (.+) - STACKTRACE: (.*))?$", RegexOptions.IgnoreCase);

            foreach (string filePath in Directory.EnumerateFiles(Path.Combine(BaseDirectory, AppName)))
            {
                using (StreamReader log_reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (log_reader.Peek() >= 0)
                    {
                        string line = log_reader.ReadLine();
                        Match logMatch = loggRegex.Match(line);
                        if (logMatch.Success)
                        {
                            LoggInfo loggInfo;
                            DateTime date = DateTime.Parse(logMatch.Groups[1].Value);
                            string text = logMatch.Groups[3].Value;
                            LoggType loggType;
                            if (!Enum.TryParse<LoggType>(logMatch.Groups[2].Value, out loggType))
                                continue;
                            
                            if (logMatch.Groups[4].Success)
                            {
                                Type type = Type.GetType(logMatch.Groups[4].Value);
                                string innerMessage = logMatch.Groups[5].Value;
                                string innerStack = logMatch.Groups[6].Value;
                                loggInfo = new LoggInfo(date, text, loggType, type, innerMessage, innerStack);
                            }
                            else
                            {
                                loggInfo = new LoggInfo(text, loggType);
                                loggInfo.Date = date;
                            }

                            loggInfos.Add(loggInfo);
                        }
                    }
                }
            }
            return loggInfos;
        }
    }
}
