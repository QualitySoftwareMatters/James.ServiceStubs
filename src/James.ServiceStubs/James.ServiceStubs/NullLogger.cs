namespace James.ServiceStubs
{
    public class NullLogger : ILogger
    {
        public void Info(string message)
        {
        }

        public void Info(string message, params object[] args)
        {
        }

        public void Warn(string message)
        {
        }

        public void Warn(string message, params object[] args)
        {
        }
    }
}