using log4net;

namespace Web_Triolingo.Logger
{
    public class Log4NetCore : ILogger
    {
        private static ILog logger = LogManager.GetLogger(typeof(Log4NetCore));

        public IDisposable BeginScope<TState>(TState state)
        {
            try
            {
                if (state is Array)
                {
                    var fileds = state as Array;
                    foreach (var filed in fileds)
                    {
                        if (filed is KeyValuePair<string, string>)
                        {
                            var prop = (KeyValuePair<string, string>)filed;
                            LogicalThreadContext.Properties[prop.Key] = prop.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(string.Format("BeginScope error {0}", e.Message));
            }
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = null;
            if (null != formatter)
            {
                message = formatter(state, exception);
            }
            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                //if (!message.Contains("Cloud.Sdk") && !message.Contains("Api.Controllers.SettingsController"))
                //{
                //    return;
                //}
                switch (logLevel)
                {
                    case LogLevel.Critical:
                        logger.Error(message);
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Trace:
                        logger.Debug(message);
                        break;
                    case LogLevel.Error:
                        logger.Error(message);
                        break;
                    case LogLevel.Information:
                        logger.Info(message);
                        break;
                    case LogLevel.Warning:
                        logger.Warn(message);
                        break;
                    default:
                        logger.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                        logger.Info(message);
                        break;
                }
            }
        }

    }
}
