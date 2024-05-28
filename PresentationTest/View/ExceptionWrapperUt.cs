using System;
using System.Collections;
using NUnit.Framework;
using Presentation.View;

namespace PresentationTest.View;

[TestFixture]
public class ExceptionWrapperUt
{
    // [Test]
    // public void ExceptionWrapper_Constructors_AllPropertiesSet()
    // {
    //     var systemUnderTest = new ExceptionWrapper("super special call site");
    //     
    //     Assert.That(systemUnderTest.CallSite, Is.EqualTo("super special call site"));
    //     Assert.That(systemUnderTest.Exception, Is.Null);
    //
    //     systemUnderTest = new ExceptionWrapper("another special call site", new NotImplementedException("foobar"));
    //     
    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(systemUnderTest.CallSite, Is.EqualTo("another special call site"));
    //         Assert.That(systemUnderTest.Exception, Is.Not.Null);
    //     });
    //     Assert.That(systemUnderTest.Exception, Is.TypeOf<NotImplementedException>());
    //     Assert.That(systemUnderTest.Exception!.Message, Is.EqualTo("foobar"));
    // }
    //
    // [Test]
    // [TestCaseSource(typeof(ExceptionWrapperToStringCases))]
    // public void ExceptionWrapper_ToString_PrintsExceptionTextCorrectly(ExceptionWrapper systemUnderTest, string expected)
    // {
    //     var actual = systemUnderTest.ToString();
    //     
    //     Assert.That(actual, Is.EqualTo(expected));
    // }

}

internal class ExceptionWrapperToStringCases : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] { new ExceptionWrapper("the callsite"), "Exception encountered at the callsite." };
        yield return new object[] { new ExceptionWrapper("the callsite", new Exception("Exception message")),
@"Exception encountered at the callsite:
Exception:
Exception message" };
        yield return new object[] { new ExceptionWrapper("the callsite",
                new Exception("Exception message", new Exception("the inner exception message"))), 
@"Exception encountered at the callsite:
Exception:
Exception message
Inner Exception:
the inner exception message" };
    }
}