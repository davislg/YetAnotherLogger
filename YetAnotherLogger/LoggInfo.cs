using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAL
{
    public class LoggInfo
    {
        public string Text { get; set; }
        public LoggType Type { get; set; }
        public DateTime Date { get; set; }
    }


    public enum LoggType
    {
        Info,
        Warning,
        Error,
        General,
    }
}
