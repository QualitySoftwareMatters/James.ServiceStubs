using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using James.ServiceStubs.CommandLine.Mono.Options;

namespace James.ServiceStubs.CommandLine
{
    public class Client
    {
        private const int DefaultPort = 1234;

        public void Run()
        {
            var port = DefaultPort;
            string filePath = null;

            var options = new OptionSet
            {
                {"p|port=", (int v) => port = v},
                {"f|filePath=", v =>  filePath = v},
            };
            options.Add("h|?|help", v => ShowHelp(options));
            options.Parse(Environment.GetCommandLineArgs());

            var uri = new Uri($"http://localhost:{port}");
            
            using (var host = GetHost(uri, filePath))
            {
                host.Start();

                Console.WriteLine($"Listening for requests at {uri.OriginalString}");
                Console.WriteLine("Hit ENTER to quit...");
                Console.WriteLine("");
                Console.ReadLine();
            }
        }

        private ServiceStubsHost GetHost(Uri uri, string filePath)
        {
            if (filePath == null)
            {
                return new ServiceStubsHost(uri);
            }

            var bootstrapper = new CustomServiceStubsBootstrapper(filePath);
            return new ServiceStubsHost(uri, bootstrapper);

        }

        private void ShowHelp(OptionSet p)
        {
            Console.WriteLine($"Usage: {Assembly.GetCallingAssembly().GetName().Name} [OPTIONS]+");
            Console.WriteLine("Host a configurable set of stubbed service endpoints.");
            Console.WriteLine($"If no port is specified, a generic port ({DefaultPort}) is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
        }
    }
}