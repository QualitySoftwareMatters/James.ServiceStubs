using System;
using System.IO;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

namespace James.ServiceStubs.UnitTests.FileTemplateProviderTests
{
    [TestFixture]
    public class given_file_does_not_exist_when_getting_content_for
    {
        private const string TemplateKey = "myTemplate";
        private ILogger _logger;
        private string _contents;

        [OneTimeSetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            var fileProvider = Substitute.For<IFileProvider>();
            var provider = new FileTemplateProvider(_logger, fileProvider);

            fileProvider.Exists(Path.Combine(Environment.CurrentDirectory, TemplateKey)).Returns(false);

            _contents = provider.GetContentsFor(TemplateKey, null);
        }

        [Test]
        public void given_file_does_not_exist_when_getting_content_for_then_should_log_error()
        {
            _logger.Received(1).Error(FileTemplateProvider.TemplateDoesNotExistFormat, Arg.Is<string>(v => v.Contains(TemplateKey)));
        }

        [Test]
        public void given_file_does_not_exist_when_getting_content_for_then_should_return_empty_content()
        {
            _contents.Should().BeEmpty();
        }
    }
}