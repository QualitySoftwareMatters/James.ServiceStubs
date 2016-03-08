namespace James.ServiceStubs
{
    public interface ILogger
    {
        void Info(string message);
        void Info(string message, params object[] args);
        void Warn(string message);
        void Warn(string message, params object[] args);
    }
}
