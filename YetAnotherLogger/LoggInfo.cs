using System;

namespace YAL
{
    public class LoggInfo
    {
        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> to Current Date and
        /// <see cref="Type"/> to LoggType.General
        /// </summary>
        public LoggInfo()
        {
            this.Type = LoggType.General;
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> to Current Date and
        /// <see cref="Type"/> to LoggType.General
        /// </summary>
        /// <param name="Text">The error message</param>
        public LoggInfo(string Text)
            : this()
        {
            this.Text = Text;
        }

        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> to Current Date
        /// </summary>
        /// <param name="Text">The error message</param>
        /// <param name="Type">The <see cref="LoggType"/> of this error</param>
        public LoggInfo(string Text, LoggType Type)
            : this(Text)
        {
            this.Type = Type;
        }

        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> to Current Date
        /// </summary>
        /// <param name="Text">The error message</param>
        /// <param name="Type">The <see cref="LoggType"/> of this error</param>
        /// <param name="Exception">The InnerException of this Error</param>
        public LoggInfo(string Text, LoggType Type, Exception Exception)
            : this(Text, Type)
        {
            this.Exception = Exception;
        }

        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> set to the Current Date and
        /// <see cref="Type"/> to LoggType.General
        /// </summary>
        /// <param name="Exception">The Exception</param>
        public LoggInfo(Exception Exception)
            : this()
        {
            this.Exception = Exception;
        }

        /// <summary>
        /// Creates a new Instance of <see cref="LoggInfo"/>
        /// With the <see cref="Date"/> set to the Current Date
        /// </summary>
        /// <param name="Exception">The Exception</param>
        /// <param name="Type">The <see cref="LoggType"/> of this Exception</param>
        public LoggInfo(Exception Exception, LoggType Type)
            : this(Exception)
        {
            this.Type = Type;
        }
        /// <summary>
        /// The error message to log.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The type of the error message
        /// </summary>
        public LoggType Type { get; set; }

        /// <summary>
        /// The date the error was caught.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The exception, or inner exception to catch.
        /// </summary>
        public Exception Exception { get; set; }
    }

    /// <summary>
    /// The LoggType to be used when writing
    /// the current line of the log.
    /// </summary>
    public enum LoggType
    {
        Info,
        Warning,
        Error,
        General,
    }
}
