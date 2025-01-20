using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.Api.JavaScript;

[TestFixture]
public class ReceiveJsonDataTests
{
    private IValidateH5pUc _validateH5pUcMock;

    [SetUp]
    public void Setup()
    {
        _validateH5pUcMock = Substitute.For<IValidateH5pUc>();
        ReceiveFromJavaScriptAdapter.ValidateH5pUc = _validateH5pUcMock;
    }

    [Test]
    public void ReceiveJsonData_CompletedWithoutParent_CallsValidateH5pWithTrue()
    {
        // Arrange
        string jsonCompletedWithoutParent = @"
        {
            ""data"": {
                ""statement"": {
                    ""verb"": {
                        ""id"": ""http://adlnet.gov/expapi/verbs/completed""
                    },
                    ""context"": {
                        ""contextActivities"": {}
                    }
                }
            }
        }";

        ReceiveFromJavaScriptAdapter.ReceiveJsonData(jsonCompletedWithoutParent);

        var expectedValidateH5pTO = new ValidateH5pTO(true);
        _validateH5pUcMock.Received(1).ValidateH5p(Arg.Is(expectedValidateH5pTO));
    }

    [Test]
    public void ReceiveJsonData_AnsweredWithParent_CallsValidateH5pWithFalse()
    {
        // Arrange
        string jsonAnsweredWithParent = @"
        {
            ""data"": {
                ""statement"": {
                    ""verb"": {
                        ""id"": ""http://adlnet.gov/expapi/verbs/answered""
                    },
                    ""context"": {
                        ""contextActivities"": {
                            ""parent"": [
                                {
                                    ""id"": ""http://example.com/parent""
                                }
                            ]
                        }
                    }
                }
            }
        }";
       
        // Act
        ReceiveFromJavaScriptAdapter.ReceiveJsonData(jsonAnsweredWithParent);

        
        
        var expectedValidateH5pTO = new ValidateH5pTO(false);
        _validateH5pUcMock.Received(1).ValidateH5p(Arg.Is(expectedValidateH5pTO));
    }

  
}

