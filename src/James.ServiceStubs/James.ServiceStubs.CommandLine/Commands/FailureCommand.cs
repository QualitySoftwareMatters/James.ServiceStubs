using System;

namespace ServiceStubs.Commands
{
    public class FailureCommand : ICommand
    {
        private readonly Exception _ex;

        public FailureCommand(Exception ex)
        {
            _ex = ex;
        }

        public int Execute(string[] args)
        {
            Console.Write("servicestubs: ");
            Console.WriteLine(_ex.Message);
            Console.WriteLine("Try 'servicestubs --help' for more information.");
            return -1;
        }
    }
}