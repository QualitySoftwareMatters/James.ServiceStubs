namespace James.ServiceStubs
{
    public interface IFileProvider
    {
        bool Exists(string path);
        string ReadAllText(string path);
    }
}