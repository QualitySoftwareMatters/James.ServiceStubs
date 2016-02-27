using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

using James.ServiceStubs.CommandLine.Mono.Options;
using James.ServiceStubs.Core;

namespace James.ServiceStubs.CommandLine
{
    class Program
    {
        public static int DefaultPort = 1234;

        static int Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                return CreateNewAppDomain();
            }

            var options = new OptionSet();
            options.Add("p|port=", (int v) => DefaultPort = v);
            options.Add("h|?|help", v => ShowHelp(options));
            options.Parse(Environment.GetCommandLineArgs());

            var uri = new Uri($"http://localhost:{DefaultPort}");
            using (var host = new ServiceStubsHost(uri))
            {
                host.Start();

                Console.WriteLine($"Listening for requests at {uri.OriginalString}");
                Console.WriteLine("Hit ENTER to quit...");
                Console.WriteLine("");
                Console.ReadLine();
            }

            return 0;
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine($"Usage: {Assembly.GetCallingAssembly().GetName().Name} [OPTIONS]+");
            Console.WriteLine("Host a configurable set of stubbed service endpoints.");
            Console.WriteLine($"If no port is specified, a generic port ({DefaultPort}) is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();
        }

        private static int CreateNewAppDomain()
        {
            Console.WriteLine("Switching to secound AppDomain for RazorEngine clean up.");
            Console.WriteLine();

            var current = AppDomain.CurrentDomain;
            var strongNames = new StrongName[0];

            var domain = AppDomain.CreateDomain(
                "MyMainDomain", null,
                current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                strongNames);
            var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);

            AppDomain.Unload(domain);
            return exitCode;
        }
    }
}
