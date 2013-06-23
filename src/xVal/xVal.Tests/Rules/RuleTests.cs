using System;
using Xunit;
using xVal.Rules;

namespace xVal.Tests.Rules
{
    public class RuleTests
    {
        [Fact]
        public void Can_Set_ErrorMessage()
        {
            var rule = new TestRule();
            rule.ErrorMessage = "Some error message";
        }

        [Fact]
        public void Can_Set_ErrorResourceType_And_Name()
        {
            var rule = new TestRule();
            rule.ErrorMessageResourceType = typeof (TestResources);
            rule.ErrorMessageResourceName = "Anything";
        }

        [Fact]
        public void Cannot_Set_ErrorResourceType_After_Setting_ErrorMessage()
        {
            var rule = new TestRule();
            
            rule.ErrorMessage = "Some error message";
            Assert.Throws<InvalidOperationException>(delegate {
                rule.ErrorMessageResourceType = typeof(TestResources);
            });
        }

        [Fact]
        public void Cannot_Set_ErrorResourceName_After_Setting_ErrorMessage()
        {
            var rule = new TestRule();

            rule.ErrorMessage = "Some error message";
            Assert.Throws<InvalidOperationException>(delegate
            {
                rule.ErrorMessageResourceName = "Anything";
            });
        }

        [Fact]
        public void Cannot_Set_ErrorMessage_After_Setting_ResourceType_And_Name()
        {
            var rule = new TestRule();
            
            rule.ErrorMessageResourceType = typeof(TestResources);
            rule.ErrorMessageResourceName = "SomeResource";
            
            Assert.Throws<InvalidOperationException>(delegate
            {
                rule.ErrorMessage = "Some error message";    
            });
        }

        [Fact]
        public void ErrorMessageOrResourceString_Returns_Null_When_No_Values_Were_Set()
        {
            var rule = new TestRule();
            Assert.Null(rule.ErrorMessageOrResourceString);
        }

        [Fact]
        public void ErrorMessageOrResourceString_Returns_ErrorMessage_When_Set()
        {
            var rule = new TestRule { ErrorMessage = "My error message" };
            Assert.Equal("My error message", rule.ErrorMessageOrResourceString);
        }

        [Fact]
        public void ErrorMessageOrResourceString_Returns_Resource_String_When_Configured()
        {
            var rule = new TestRule { ErrorMessageResourceType = typeof(TestResources), ErrorMessageResourceName = "SomeResource" };
            Assert.Equal("This is a resource string", rule.ErrorMessageOrResourceString);
        }

        private class TestRule : Rule
        {
            public TestRule() : base("Test rule")
            {
            }
        }

        private static class TestResources
        {
            public static string SomeResource { get { return "This is a resource string"; } }
        }
    }
}