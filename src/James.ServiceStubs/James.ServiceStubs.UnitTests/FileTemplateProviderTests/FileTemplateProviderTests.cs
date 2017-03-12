using System;
using System.IO;

using FluentAssertions;

using James.ServiceStubs.Razor;

using NSubstitute;

using NUnit.Framework;

namespace James.ServiceStubs.UnitTests.FileTemplateProviderTests
{
    [TestFixture]
    public class FileTemplateProviderTests
    {
        [Test]
        public void given_file_exists_when_getting_content_for_then_should_return_content()
        {
            var logger = Substitute.For<ILogger>();
            var fileProvider = Substitute.For<IFileProvider>();
            var provider = new FileTemplateProvider(logger, fileProvider);

            const string templateKey = "myTemplate";
            const string expectedContent = "This is the expected content.";

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateKey);
            fileProvider.Exists(filePath).Returns(true);
            fileProvider.ReadAllText(filePath).Returns(expectedContent);

            var contents = provider.GetContentsFor(templateKey, null);

            contents.Should().Be(expectedContent);
        }

        [Test]
        public void given_file_exists_for_custom_path_when_getting_content_for_then_should_return_content()
        {
            const string customPath = "customPath";
            var logger = Substitute.For<ILogger>();
            var fileProvider = Substitute.For<IFileProvider>();
            var provider = new FileTemplateProvider(logger, fileProvider, customPath);

            const string expectedContent = "This is the expected content.";
            const string templateKey = "myTemplate";

            fileProvider.Exists(Path.Combine(customPath, templateKey)).Returns(true);
            fileProvider.ReadAllText(Path.Combine(customPath, templateKey)).Returns(expectedContent);
        
            var contents = provider.GetContentsFor(templateKey, null);
            contents.Should().Be(expectedContent);
        }
    }
}
