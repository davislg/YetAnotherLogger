using System;
using System.IO;
using System.Collections.ObjectModel;

namespace YAL
{
    public abstract class BaseLogger
    {
        /// <summary>
        /// The name of the Application Folder to be used.
        /// </summary>
        public static string AppName;
        /// <summary>
        /// The File name to log in.
        /// </summary>
        private static string fileName;
        /// <summary>
        /// The directory to save the log file in, excluding <see cref="AppName"/>.
        /// </summary>
        public static string BaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        protected static BaseLogger _default;
        private static ObservableCollection<LoggInfo> loggInfos;

        /// <summary>
        /// Creates a new <see cref="FileLogger"/>
        /// </summary>
        /// <returns>The created <see cref="FileLogger"/></returns>
        public static BaseLogger Create()
        {
            return Create("FileLogger");
        }

        /// <summary>
        /// The <see cref="BaseLogger"/>.
        /// NOTE: It will create a new <see cref="FileLogger"/>
        /// if no logger has been created previously.
        /// </summary>
        public static BaseLogger Default
        {
            get
            {
                if (_default == null)
                    return Create();

                return _default;
            }
        }

        public static string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value.Replace('/', '-').Replace(':', '-');
            }
        }

        public static ObservableCollection<LoggInfo> LoggInfos
        {
            get
            {
                if (loggInfos == null)
                    loggInfos = Default.GetLogg();
                return loggInfos;
            }
        }

        /// <summary>
        /// Creates a new <see cref="BaseLogger"/>
        /// </summary>
        /// <param name="loggerName">
        /// The name of the logger to create.
        /// Valid values:
        /// FileLogger and XmlLogger
        /// </param>
        /// <returns>The new <see cref="BaseLogger"/></returns>
        public static BaseLogger Create(string loggerName)
        {
            if (string.Compare(loggerName, "FileLogger", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _default = new FileLogger();
            }
            else if (string.Compare(loggerName, "XmlLogger", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _default = new XmlLogger();
            }
            return _default;

            throw new ArgumentException("The logger " + loggerName + " was not found.");
        }

        protected void CreateDirIfNotExist()
        {
            string path = Path.Combine(BaseDirectory, AppName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Logs a new line of text
        /// </summary>
        /// <param name="type">The type of the error to log</param>
        /// <param name="text">The error message to log.</param>
        /// <param name="innerException">The InnerException of this error</param>
        public abstract void Log(LoggType type, string text, Exception innerException = null);

        /// <summary>
        /// Logs an error
        /// </summary>
        /// <param name="loggInfo">The <see cref="LoggInfo"/> to log from.</param>
        public abstract void Log(LoggInfo loggInfo);

        protected abstract ObservableCollection<LoggInfo> GetLogg();

        protected static void AddToObservable(LoggInfo loggInfo)
        {
            LoggInfos.Add(loggInfo);
        }
    }
}
