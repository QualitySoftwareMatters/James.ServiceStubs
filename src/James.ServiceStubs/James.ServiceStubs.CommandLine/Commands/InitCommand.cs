using System;
using System.IO;

using James.ServiceStubs;

namespace ServiceStubs.Commands
{
    public class InitCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IDirectoryProvider _directoryProvider;

        public InitCommand(ILogger logger, IFileProvider fileProvider, IDirectoryProvider directoryProvider)
        {
            _logger = logger;
            _fileProvider = fileProvider;
            _directoryProvider = directoryProvider;
        }

        public int Execute(string[] args)
        {
            InitializeRoutes();
            InitializeSampleTemplate();

            _logger.Info("Initialization complete.");
            return 0;
        }

        private void InitializeRoutes()
        {
            var routesContent = new EmbeddedFileProvider().GetContentsForFile("Commands.InitContent.routes.json");
            const string fileName = "routes.json";
            var routesFilePath = Path.Combine(Environment.CurrentDirectory, fileName);

            if (_fileProvider.Exists(routesFilePath))
            {
                _logger.Info($"{fileName} file already exists.");
            }
            else
            {
                File.WriteAllText(routesFilePath, routesContent);
                _logger.Info($"{fileName} file created.");
            }
        }

        private void InitializeSampleTemplate()
        {
            var templateContent = new EmbeddedFileProvider().GetContentsForFile("Commands.InitContent.Sample.template");
            var folder = "Templates";
            var fileName = "Sample.template";

            var folderPath = Path.Combine(Environment.CurrentDirectory, folder);
            var templatePath = Path.Combine(folderPath, fileName);

            if (!_directoryProvider.Exists(folderPath))
            {
                _directoryProvider.CreateDirectory(folderPath);
            }

            if (_fileProvider.Exists(templatePath))
            {
                _logger.Info($"{folder}/{fileName} file already exists.");
            }
            else
            {
                File.WriteAllText(templatePath, templateContent);
                _logger.Info($"{fileName} created under the {folder} folder.");
            }
        }
    }

    public interface IDirectoryProvider
    {
        bool Exists(string path);
        void CreateDirectory(string path);
    }

    public class DirectoryProvider : IDirectoryProvider
    {
        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}