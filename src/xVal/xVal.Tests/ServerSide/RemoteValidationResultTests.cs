using Xunit;
using xVal.ServerSide;

namespace xVal.Tests.ServerSide
{
    public class RemoteValidationResultTests
    {
        private const string JsonTrue = "true";
        private const string JsonFalse = "false";

        [Fact]
        public void SuccessResult_RendersJsonTrue()
        {
            Assert.Equal(JsonTrue, RemoteValidationResult.Success.ToString());
        }

        [Fact]
        public void FailureResult_WithNullErrorMessage_RendersJsonFalse()
        {
            Assert.Equal(JsonFalse, RemoteValidationResult.Failure(null).ToString());
        }

        [Fact]
        public void FailureResult_WithEmptyErrorMessage_RendersJsonFalse()
        {
            Assert.Equal(JsonFalse, RemoteValidationResult.Failure(string.Empty).ToString());
        }

        [Fact]
        public void FailureResult_WithNoErrorMessage_RendersJsonFalse()
        {
            Assert.Equal(JsonFalse, RemoteValidationResult.Failure().ToString());
        }

        [Fact]
        public void FailureResult_WithErrorMessage_RendersJsonString()
        {
            Assert.Equal(@"""test\u0027error\"" message""", RemoteValidationResult.Failure(@"test'error"" message").ToString());
        }
    }
}