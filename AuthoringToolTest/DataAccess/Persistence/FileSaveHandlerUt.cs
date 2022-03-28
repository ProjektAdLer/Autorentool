using System;
using AuthoringTool.DataAccess.Persistence;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.Persistence;

[TestFixture]
public class FileSaveHandlerUt
{
    private class TestNotSerializable
    {
        public TestNotSerializable()
        {
            Name = "hello";
        }
        public string Name { get; set; }
    }

    [Serializable]
    private class TestNoParameterlessConstructor
    {
        public TestNoParameterlessConstructor(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
    [Test]
    public void Persistence_Constructor_FailsIfTypeNotSerializable()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new FileSaveHandler<TestNotSerializable>(null!));
        Assert.That(ex!.Message, Is.EqualTo($"Type {nameof(TestNotSerializable)} is not serializable."));
    }
    
    [Test]
    public void Persistence_Constructor_FailsIfTypeDoesNotHaveParameterlessConstructor()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new FileSaveHandler<TestNoParameterlessConstructor>(null!));
        Assert.That(ex!.Message, Is.EqualTo($"Type {nameof(TestNoParameterlessConstructor)} has no required parameterless constructor."));
    }
}