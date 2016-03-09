using System;

namespace James.ServiceStubs
{
    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            WriteLine("info", message);
        }

        public void Info(string message, params object[] args)
        {
            Info(string.Format(message, args));
        }

        public void Warn(string message)
        {
            WriteLine("warning", message);
        }

        public void Warn(string message, params object[] args)
        {
            Warn(string.Format(message, args));
        }

        public void Error(string message)
        {
            WriteLine("error", message);
        }

        public void Error(string message, params object[] args)
        {
            Error(string.Format(message, args));
        }

        private void WriteLine(string logLevel, string message)
        {
            Console.Out.WriteLine($"{logLevel.ToUpper()}:  {message}");
        }
    }
}