using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace ServiceStubs.Commands
{
    public class NewAppDomainCommand : ICommand
    {
        public int Execute(string[] args)
        {
            var current = AppDomain.CurrentDomain;
            var strongNames = new StrongName[0];

            Console.WriteLine("Switching to second AppDomain for RazorEngine clean up.");
            Console.WriteLine();

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