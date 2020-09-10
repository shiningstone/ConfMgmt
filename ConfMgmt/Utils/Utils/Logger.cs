using System;
using System.IO;
using System.Linq;
using System.Reflection;

using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Layout.Pattern;
using log4net.Repository.Hierarchy;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = "config", Watch = true)]
namespace Utils
{
    public class Logger
    {
        public delegate void InformUpdate(string level, string log);
        public static InformUpdate Observer;
        #region configuration
        private static readonly string mStation = "localhost";
        private static readonly string mUser = "bo.jiang";
        #endregion

        readonly protected ILog mLog;
        public Logger(Type type = null)
        {
            mLog = (type == null ? LogManager.GetLogger("") : LogManager.GetLogger(type));
            if (mLog == null)
            {
                throw new Exception($"Failed to get logger({type})");
            }
        }
        public Logger(string type)
        {
            mLog = LogManager.GetLogger(type);
            if (mLog == null)
            {
                throw new Exception($"Failed to get logger({type})");
            }
        }
        public virtual void Debug(string msg)
        {
            mLog.Debug(msg);
        }
        public virtual void Info(string msg)
        {
            mLog.Info(msg);
            Observer?.Invoke("INFO", msg);
        }
        public virtual void Warn(string msg, Exception exception = null)
        {
            mLog.Warn(new LogContent(mStation, mUser, msg), exception);
            Observer?.Invoke("WARN", msg);
        }
        public virtual void Error(string msg, Exception exception = null)
        {
            mLog.Error(new LogContent(mStation, mUser, msg), exception);
            Observer?.Invoke("ERROR", msg);
        }

        public static void CreateNewFile()
        {
            var rootAppender = ((Hierarchy)LogManager.GetRepository()).Root.Appenders.OfType<FileAppender>().FirstOrDefault();
            string filename = rootAppender != null ? rootAppender.File : string.Empty;
            if (!string.IsNullOrEmpty(filename))
            {
                File.Copy(filename, Calc.AddPostfix(filename, $"-{DateTime.Now.ToString("HHmmss")}"));
                File.WriteAllText(filename, "");
            }
        }
    }
    #region database log content
    public class LogContent
    {
        public LogContent(string station, string user, string description)
        {
            Station = station;
            User = user;
            Message = description;
        }
        public string Station { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return Message;
        }
    }
    public class MyLayout : PatternLayout
    {
        public MyLayout()
        {
            this.AddConverter("property", typeof(LogInfoPatternConverter));
        }
    }
    public class LogInfoPatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
        private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            return propertyValue;
        }
    }
    #endregion
}
