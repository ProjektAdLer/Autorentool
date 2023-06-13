using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;
using Shared.Configuration;

namespace SharedTest.Configuration
{
    [TestFixture]
    public class ApplicationConfigurationTests
    {
        private ILogger<ApplicationConfiguration> _logger;
        private MockFileSystem _fileSystem;
        private string _folderPath;
        private string _filePath;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<ApplicationConfiguration>>();
            _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AdLerAuthoring");
            _filePath = Path.Combine(_folderPath, "ApplicationConfig.json");
            _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    _folderPath, new MockDirectoryData()
                }
            });
        }

        [Test]
        public void Constructor_InjectsDependencies()
        {
            var systemUnderTest = CreateApplicationConfiguration();
            
            Assert.Multiple(() =>
            {
                Assert.That(systemUnderTest.Logger, Is.EqualTo(_logger));
                Assert.That(systemUnderTest.FileSystem, Is.EqualTo(_fileSystem));
            });
        }

        [Test]
        public void Constructor_NoConfigFileFound_GeneratesDefaultConfigAndSavesToFile()
        {
            // Arrange
            var expectedDefaultConfig = new ObservableDictionary<string, string> { { "BackendBaseUrl", "" } };
            var expectedJson = JsonSerializer.Serialize(expectedDefaultConfig);

            // Act
            var configuration = new ApplicationConfiguration(_logger, _fileSystem);

            // Assert
            var mockFileData = _fileSystem.GetFile(_filePath);
            Assert.Multiple(() =>
            {
                Assert.That(configuration.Configuration, Is.EqualTo(expectedDefaultConfig));
                Assert.That(mockFileData, Is.Not.Null);
            });
            Assert.That(mockFileData.Contents, Is.EqualTo(expectedJson));
        }

        [Test]
        public void Constructor_ConfigFileFound_LoadsConfigurationAndSavesToFile()
        {
            // Arrange
            var expectedConfig = new ObservableDictionary<string, string>
                { { "BackendBaseUrl", "http://example.com" } };
            var json = JsonSerializer.Serialize(expectedConfig);
            _fileSystem.AddFile(_filePath, new MockFileData(json));

            // Act
            var configuration = new ApplicationConfiguration(_logger, _fileSystem);

            // Assert
            Assert.That(configuration.Configuration, Is.EqualTo(expectedConfig));
        }

        [Test]
        public void Indexer_GetValue_ReturnsValueFromConfiguration()
        {
            // Arrange
            var expectedConfig = new ObservableDictionary<string, string>
                { { "BackendBaseUrl", "http://example.com" } };
            var json = JsonSerializer.Serialize(expectedConfig);
            _fileSystem.AddFile(_filePath, new MockFileData(json));
            
            var configuration = CreateApplicationConfiguration();

            // Act
            var value = configuration["BackendBaseUrl"];

            // Assert
            Assert.That(value, Is.EqualTo("http://example.com"));
        }

        [Test]
        public void Indexer_SetValue_SetsValueInConfigurationAndSavesToFile()
        {
            // Arrange
            var expectedConfig = new ObservableDictionary<string, string>
                { { "BackendBaseUrl", "http://example.com" } };
            var json = JsonSerializer.Serialize(expectedConfig);
            _fileSystem.AddFile(_filePath, new MockFileData(json));
            
            var configuration = CreateApplicationConfiguration();

            // Act
            configuration["BackendBaseUrl"] = "http://example.co.uk";

            // Assert
            var mockFileData = _fileSystem.GetFile(_filePath);
            Assert.Multiple(() =>
            {
                Assert.That(configuration.Configuration["BackendBaseUrl"], Is.EqualTo("http://example.co.uk"));
                Assert.That(mockFileData, Is.Not.Null);
            });
            Assert.That(mockFileData.Contents, Is.EqualTo(JsonSerializer.Serialize(new Dictionary<string, string>
            {
                { "BackendBaseUrl", "http://example.co.uk" }
            })));
        }

        private ApplicationConfiguration CreateApplicationConfiguration()
        {
            return new ApplicationConfiguration(_logger, _fileSystem);
        }
    }
}