using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AuthoringToolLib.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolLibTest.Components;

[TestFixture]
public class DropZoneUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void DefaultConstructor_AllPropertiesInitialized()
    {
        Action<Tuple<string, Stream>> onNewStream = _ => { };

        var systemUnderTest = CreateRenderedDropZoneComponent(onNewStream);

        Assert.That(systemUnderTest.Instance.OnNewStream,
            Is.EqualTo(EventCallback.Factory.Create(
                onNewStream.Target ?? throw new InvalidOperationException("onNewStream.Target is null"), onNewStream)));
    }

    [Test]
    public void OnChange_OnNewStreamTriggered()
    {
        Tuple<string, Stream>? onNewStreamEventTriggered = null;
        Action<Tuple<string, Stream>> onNewStream = e => { onNewStreamEventTriggered = e; };
        const string testFileName = "TestFileName";

        var fileToUpload = new InputFileChangeEventArgs(new[]
        {
            new TestFile {Name = testFileName}
        });

        var systemUnderTest = CreateRenderedDropZoneComponent(onNewStream);

        var inputComponent = systemUnderTest.FindComponent<InputFile>().Instance;
        systemUnderTest.InvokeAsync(() => inputComponent.OnChange.InvokeAsync(fileToUpload));

        Debug.Assert(onNewStreamEventTriggered != null, nameof(onNewStreamEventTriggered) + " != null");
        Assert.That(onNewStreamEventTriggered.Item1, Is.EqualTo(testFileName));
    }

    [Test]
    public void OnChange_StreamToLarge_ThrowException()
    {
        Tuple<string, Stream>? onNewStreamEventTriggered = null;
        Action<Tuple<string, Stream>> onNewStream = e => { onNewStreamEventTriggered = e; };
        const string testFileName = "TestFileName";

        var fileToUpload = new InputFileChangeEventArgs(new[]
        {
            new TestFile {Name = testFileName, IsSizeToLarge = true}
        });

        var systemUnderTest = CreateRenderedDropZoneComponent(onNewStream);
        systemUnderTest.Instance.Logger = Substitute.For<ILogger<DropZone>>();

        var inputComponent = systemUnderTest.FindComponent<InputFile>().Instance;
        systemUnderTest.InvokeAsync(() => inputComponent.OnChange.InvokeAsync(fileToUpload));

        Assert.That(onNewStreamEventTriggered, Is.Null);
        systemUnderTest.Instance.Logger.ReceivedWithAnyArgs().LogDebug(default);
    }

    private record TestFile : IBrowserFile
    {
        public string Name { get; init; } = string.Empty;
        public DateTimeOffset LastModified { get; init; } = DateTimeOffset.MinValue;
        public long Size { get; init; } = long.MinValue;
        public string ContentType { get; init; } = string.Empty;
        public bool IsSizeToLarge { get; init; }

        public Stream OpenReadStream(long maxAllowedSize = 2147483648L, CancellationToken cancellationToken = default)
        {
            if (IsSizeToLarge)
            {
                return new MemoryStream(new byte[2147483648L]);
            }

            return new MemoryStream(Array.Empty<byte>());
        }
    }

    private IRenderedComponent<DropZone> CreateRenderedDropZoneComponent(
        Action<Tuple<string, Stream>>? onNewStream = null)
    {
        onNewStream ??= _ => { };
        return _testContext.RenderComponent<DropZone>(parameters => parameters
            .Add(p => p.OnNewStream, onNewStream)
        );
    }
}