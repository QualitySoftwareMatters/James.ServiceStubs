using System;

using James.ServiceStubs;

namespace ServiceStubs.Commands
{
    public class StartCommand : ICommand
    {
        private readonly int _port;
        private readonly string _filePath;

        public StartCommand(int port, string filePath)
        {
            _port = port;
            _filePath = filePath;
        }

        public int Execute(string[] args)
        {
            var uri = new Uri($"http://localhost:{_port}");
            
            using (var host = GetHost(uri, _filePath))
            {
                host.Start();

                Console.WriteLine($"Listening for requests at {uri.OriginalString}");
                Console.WriteLine("Hit ENTER to quit...");
                Console.WriteLine("");
                Console.ReadLine();
            }

            return 0;
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
    }
}