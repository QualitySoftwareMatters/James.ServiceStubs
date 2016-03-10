namespace ServiceStubs.Commands
{
    public interface ICommand
    {
        int Execute(string[] args);
    }
}