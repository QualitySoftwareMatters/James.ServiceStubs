using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

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

            var client = new Client();
            client.Run();

            return 0;
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
