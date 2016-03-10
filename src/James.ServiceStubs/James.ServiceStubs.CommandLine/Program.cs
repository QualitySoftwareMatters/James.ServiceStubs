using System;
using System.Linq;

using James.ServiceStubs;

using Nancy.TinyIoc;

using ServiceStubs.Commands;
using ServiceStubs.Mono.Options;

namespace ServiceStubs
{
    class Program
    {
        public static int DefaultPort = 1234;

        static int Main(string[] args)
        {
            var container = GetContainer();
            args = Environment.GetCommandLineArgs();
            ICommand command;
            
            var port = DefaultPort;
            string filePath = null;
            var initialize = false;
            var showHelp = false;

            var options = new OptionSet
            {
                { "f|filePath=", "the path for configuration and template files.  \r\n(default: current directory)", v => filePath = v },
                { "i|init", "initialize configuration and sample template files.", v => initialize = v != null },
                { "p|port=", "the port that servicestubs listens on. \r\n(default:  1234)", (int v) => port = v },
                { "h|?|help", "show help", v => showHelp = v != null }
            };

            try
            {
                options.Parse(args);
            }
            catch (OptionException ex)
            {
                return new FailureCommand(ex).Execute(args);
            }

            if (initialize)
            {
                command = container.Resolve<InitCommand>();
            }
            else
            {
                if (showHelp)
                {
                    var overloads = new NamedParameterOverloads { { "options", options }, { "defaultPort", DefaultPort } };
                    command = container.Resolve<ShowHelpCommand>(overloads);
                }
                else
                {
                    if (AppDomain.CurrentDomain.IsDefaultAppDomain())
                    {
                        command = new NewAppDomainCommand();
                    }
                    else
                    {
                        var overloads = new NamedParameterOverloads { { "port", port }, { "filePath", filePath } };
                        command = container.Resolve<StartCommand>(overloads);
                    }
                }
            }

            return command.Execute(args);
        }

        private static TinyIoCContainer GetContainer()
        {
            var container = new TinyIoCContainer();
            container.AutoRegister();
            container.Register<ILogger, ConsoleLogger>();
            return container;
        }
    }
}
