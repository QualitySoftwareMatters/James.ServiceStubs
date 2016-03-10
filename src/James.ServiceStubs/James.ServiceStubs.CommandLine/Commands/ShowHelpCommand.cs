using System;
using System.Reflection;

using ServiceStubs.Mono.Options;

namespace ServiceStubs.Commands
{
    public class ShowHelpCommand : ICommand
    {
        private readonly OptionSet _options;
        private readonly int _defaultPort;

        public ShowHelpCommand(OptionSet options, int defaultPort)
        {
            _options = options;
            _defaultPort = defaultPort;
        }

        public int Execute(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine($"Usage: {Assembly.GetCallingAssembly().GetName().Name.ToLower()} [OPTIONS]+");

            Console.WriteLine();
            Console.WriteLine("Host a configurable set of stubbed service endpoints based on razor templates.");
            Console.WriteLine($"If no port is specified, a generic port ({_defaultPort}) is used.");

            Console.WriteLine();
            Console.WriteLine("Options:");

            Console.WriteLine();
            _options.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();

            return 0;
        }
    }
}