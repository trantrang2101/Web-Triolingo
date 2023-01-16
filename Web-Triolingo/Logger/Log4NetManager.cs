using log4net.Config;
using System.Reflection;

namespace Web_Triolingo.Logger
{
    public class Log4NetManager : ILoggerProvider
    {
        public Log4NetManager(string filename = "log4net.config")
        {
            var logRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            var file = new FileInfo(filename);
            XmlConfigurator.Configure(logRepository, file);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Log4NetCore();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            //cleanup
        }

    }
}
