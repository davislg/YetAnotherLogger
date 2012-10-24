using System;
using System.IO;
using System.Text;
using System.Xml;

namespace YAL
{
    public class XmlLogger : Logger
    {
        public static bool Indent = true;

        internal XmlLogger() { }

        /// <summary>
        /// The <see cref="Logger"/>.
        /// NOTE: It will create a new <see cref="XmlLogger"/>
        /// if no logger has been created previously or current logger
        /// isn't a <see cref="XmlLogger"/>.
        /// </summary>
        public new static Logger Default
        {
            get
            {
                if (_default == null || !(_default is FileLogger))
                    _default = new FileLogger();
                return _default;
            }
        }

        /// <summary>
        /// Creates a new <see cref="XmlLogger"/>
        /// </summary>
        /// <returns>The created <see cref="FileLogger"/></returns>
        public new static Logger Create()
        {
            return Create("XmlLogger");
        }

        /// <summary>
        /// Logs a new line of text to an xml file
        /// </summary>
        /// <param name="type">The type of the error to log</param>
        /// <param name="text">The error message to log.</param>
        /// <param name="innerException">The InnerException of this error</param>
        public override void Log(LoggType type, string text, Exception innerException = null)
        {
            LoggInfo loggInfo = new LoggInfo(text, type, innerException);
            Log(loggInfo);
        }

        /// <summary>
        /// Logs an error to an xml file
        /// </summary>
        /// <param name="loggInfo">The <see cref="LoggInfo"/> to log from.</param>
        public override void Log(LoggInfo loggInfo)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = Indent;
            string filePath = Path.Combine(BaseDirectory, AppName, FileName);
            bool exists = File.Exists(filePath);
            AddToObservable(loggInfo);
            XmlDocument document = new XmlDocument();
            XmlNode rootNode = document.CreateElement("logs");
            if (exists)
            {
                document.Load(filePath);
            }
            else
            {
                rootNode = document.CreateElement("logs");
                document.AppendChild(rootNode);
            }

            XmlNode logNode = document.CreateElement("log");
            XmlAttribute dateAttr = document.CreateAttribute("date");
            dateAttr.Value = loggInfo.Date.ToString();
            XmlAttribute typeAttr = document.CreateAttribute("type");
            typeAttr.Value = loggInfo.Type.ToString();
            logNode.Attributes.Append(dateAttr);
            logNode.Attributes.Append(typeAttr);

            if (!String.IsNullOrWhiteSpace(loggInfo.Text))
            {
                XmlNode textNode = document.CreateElement("text");
                textNode.InnerText = loggInfo.Text;
                logNode.AppendChild(textNode);
            }
            if (!String.IsNullOrWhiteSpace(loggInfo.StackTrace))
            {
                XmlNode stackNode = document.CreateElement("stackTrace");
                stackNode.InnerText = loggInfo.StackTrace;
                logNode.AppendChild(stackNode);
            }
            if (loggInfo.InnerException != null)
            {
                XmlNode innerNode = document.CreateElement("innerexception");
                if (loggInfo.InnerException.ExceptionType != null)
                {
                    XmlAttribute exTypeAttr = document.CreateAttribute("extype");
                    exTypeAttr.Value = loggInfo.InnerException.ExceptionType.ToString();
                    innerNode.Attributes.Append(exTypeAttr);
                }
                if (!String.IsNullOrWhiteSpace(loggInfo.InnerException.Message))
                {
                    XmlNode innerMessage = document.CreateElement("innermessage");
                    innerMessage.InnerText = loggInfo.InnerException.Message;
                    innerNode.AppendChild(innerMessage);
                }
                if (!String.IsNullOrWhiteSpace(loggInfo.InnerException.StackTrace))
                {
                    XmlNode innerStac = document.CreateElement("innerstacktrace");
                    innerStac.InnerText = loggInfo.InnerException.StackTrace;
                    innerNode.AppendChild(innerStac);
                }

                logNode.AppendChild(innerNode);
            }


            document.DocumentElement.AppendChild(logNode);

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                document.Save(writer);
            }
        }

        protected override System.Collections.ObjectModel.ObservableCollection<LoggInfo> GetLogg()
        {
            System.Collections.ObjectModel.ObservableCollection<LoggInfo> loggInfos = new System.Collections.ObjectModel.ObservableCollection<LoggInfo>();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            settings.IgnoreProcessingInstructions = true;
            if (CreateDirIfNotExist())
                foreach (string filePath in Directory.EnumerateFiles(Path.Combine(BaseDirectory, AppName), "*" + Path.GetExtension(FileName)))
                {
                    using (XmlReader logReader = XmlReader.Create(filePath, settings))
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(logReader);

                        foreach (XmlNode logNode in document["logs"].ChildNodes)
                        {
                            LoggInfo loggInfo = new LoggInfo();
                            loggInfo.Date = DateTime.Parse(logNode.Attributes["date"].InnerText);
                            LoggType loggType;
                            if (!Enum.TryParse<LoggType>(logNode.Attributes["type"].InnerText, out loggType))
                                continue;
                            loggInfo.Type = loggType;
                            if (logNode["text"] != null)
                                loggInfo.Text = logNode["text"].InnerText;

                            if (logNode["stackTrace"] != null)
                                loggInfo.StackTrace = logNode["stackTrace"].InnerText;

                            if (logNode["innerexception"] != null)
                            {
                                XmlNode innerNode = logNode["innerexception"];
                                loggInfo.InnerException = new InnerException();
                                if (innerNode.Attributes["extype"] != null)
                                    loggInfo.InnerException.ExceptionType = Type.GetType(innerNode.Attributes["extype"].InnerText);
                                if (innerNode["innermessage"] != null)
                                    loggInfo.InnerException.Message = innerNode["innermessage"].InnerText;
                                if (innerNode["innerstacktrace"] != null)
                                    loggInfo.InnerException.StackTrace = innerNode["innerstacktrace"].InnerText;
                            }

                            loggInfos.Add(loggInfo);
                        }
                    }
                }

            return loggInfos;
        }
    }
}
