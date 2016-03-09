using System;
using System.Collections.Generic;
using System.IO;

namespace James.ServiceStubs
{
    public class FileTemplateProvider : ITemplateProvider
    {
        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        private readonly string _filePath;
        public const string TemplateDoesNotExistFormat = "The path configured for the template({0} does not exist.  Empty content will be returned.";

        public FileTemplateProvider(ILogger logger, IFileProvider fileProvider, string filePath)
        {
            _filePath = filePath;
            _logger = logger;
            _fileProvider = fileProvider;
        }

        public FileTemplateProvider(ILogger logger, IFileProvider fileProvider)
            : this(logger, fileProvider, Environment.CurrentDirectory)
        {
        }

        public string GetContentsFor(string templateKey, IDictionary<string, object> parameters)
        {
            var templatePath = Path.Combine(_filePath, templateKey);

            if (_fileProvider.Exists(templatePath))
            {
                return _fileProvider.ReadAllText(templatePath);
            }

            _logger.Error(TemplateDoesNotExistFormat, templatePath);
            return string.Empty;
        }
    }
}