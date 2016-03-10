using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ServiceStubs.Commands
{
    public class EmbeddedFileProvider
    {
        public string GetContentsForFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var rootNamespace = assembly.FullName.Split(',').FirstOrDefault();
            var fullName = $"{rootNamespace}.{fileName}";
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x == fullName);

            if (resourceName == null)
                throw new ArgumentException("File is not found.", nameof(fileName));

            using (var reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}