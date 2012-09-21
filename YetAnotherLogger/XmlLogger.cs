using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAL
{
    public class XmlLogger : BaseLogger
    {

        public override void Log(LoggType type, string text, Exception innerException = null)
        {
            throw new NotImplementedException();
        }

        public override void Log(LoggInfo loggInfo)
        {
            throw new NotImplementedException();
        }

        protected override System.Collections.ObjectModel.ObservableCollection<LoggInfo> GetLogg()
        {
            throw new NotImplementedException();
        }
    }
}
