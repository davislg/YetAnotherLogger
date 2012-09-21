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
            if (Exception != null)
            {
                this.InnerException = new InnerException();
                InnerException.ExceptionType = Exception.GetType();
                InnerException.Message = Exception.Message;
                InnerException.StackTrace = Exception.StackTrace;
            }
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
            if (Exception != null)
            {
                this.InnerException = new InnerException();
                InnerException.ExceptionType = Exception.GetType();
                InnerException.Message = Exception.Message;
                InnerException.StackTrace = Exception.StackTrace;
            }
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

        public LoggInfo(DateTime date, string text, LoggType Type, Type InnerType, string InnerMessage, string InnerStack)
            : this(text, Type)
        {
            this.Date = date;
            this.InnerException = new InnerException();
            this.InnerException.ExceptionType = InnerType;
            this.InnerException.Message = InnerMessage;
            this.InnerException.StackTrace = InnerStack;
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
        /// The Inner exception to log.
        /// </summary>
        public InnerException InnerException { get; set; }
    }

    /// <summary>
    /// The inner exception to log.
    /// </summary>
    public sealed class InnerException
    {
        internal InnerException() { }

        /// <summary>
        /// The type of the exception.
        /// </summary>
        public Type ExceptionType { get; set; }

        /// <summary>
        /// The Message of the Inner Exception
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The StackTrace of the Inner Exception
        /// </summary>
        public string StackTrace { get; set; }
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
