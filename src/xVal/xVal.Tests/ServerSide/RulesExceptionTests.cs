using System.Web.Mvc;
using Xunit;
using xVal.ServerSide;
using System.Linq;

namespace xVal.Tests.ServerSide
{
    public class RulesExceptionTests
    {
        [Fact]
        public void Extends_Exception()
        {
            System.Exception ex = new RulesException("prop", "error");
        }

        [Fact]
        public void Can_Construct_With_Enumerable_Of_ErrorInfo()
        {
            var errors = new[] {
                new ErrorInfo("myProp", "myError"),
                new ErrorInfo("anotherProp", "anotherError", this)
            };
            var ex = new RulesException(errors);
            Assert.Same(errors, ex.Errors);
        }

        [Fact]
        public void Can_Construct_With_Single_ErrorInfo()
        {
            var someObj = new object();
            var ex1 = new RulesException("p1", "e1");
            var ex2 = new RulesException("p2", "e2", someObj);
            Assert.Equal("p1", ex1.Errors.ToList()[0].PropertyName);
            Assert.Equal("e1", ex1.Errors.ToList()[0].ErrorMessage);
            Assert.Null(ex1.Errors.ToList()[0].Object);
            Assert.Equal("p2", ex2.Errors.ToList()[0].PropertyName);
            Assert.Equal("e2", ex2.Errors.ToList()[0].ErrorMessage);
            Assert.Same(someObj, ex2.Errors.ToList()[0].Object);
        }

        [Fact]
        public void Populates_ModelState_Without_Prefix()
        {
            // Arrange
            var someObj = new object();
            var ex = new RulesException(new[] {
                new ErrorInfo("p1", "e1"),
                new ErrorInfo("p2", "e2", someObj),
                new ErrorInfo("p2", "e3"),
                new ErrorInfo("p2", "e4", someObj),
                new ErrorInfo("p3", "e5", someObj)
            });
            var modelState = new ModelStateDictionary();

            // Act
            ex.AddModelStateErrors(modelState, null, x => x.Object == someObj);

            // Assert
            Assert.Equal(2, modelState.Keys.Count());
            Assert.Equal(2, modelState["p2"].Errors.Count);
            Assert.Equal(1, modelState["p3"].Errors.Count);
            Assert.Equal("e2", modelState["p2"].Errors[0].ErrorMessage);
            Assert.Equal("e4", modelState["p2"].Errors[1].ErrorMessage);
            Assert.Equal("e5", modelState["p3"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void Populates_ModelState_With_Prefix()
        {
            // Arrange
            var someObj = new object();
            var ex = new RulesException(new[] {
                new ErrorInfo("p1", "e1"),
                new ErrorInfo("p2", "e2", someObj),
                new ErrorInfo("p2", "e3"),
                new ErrorInfo("p2", "e4", someObj),
                new ErrorInfo("p3", "e5", someObj)
            });
            var modelState = new ModelStateDictionary();

            // Act
            ex.AddModelStateErrors(modelState, "my.prefix", x => x.Object == someObj);

            // Assert
            Assert.Equal(2, modelState.Keys.Count());
            Assert.Equal(2, modelState["my.prefix.p2"].Errors.Count);
            Assert.Equal(1, modelState["my.prefix.p3"].Errors.Count);
            Assert.Equal("e2", modelState["my.prefix.p2"].Errors[0].ErrorMessage);
            Assert.Equal("e4", modelState["my.prefix.p2"].Errors[1].ErrorMessage);
            Assert.Equal("e5", modelState["my.prefix.p3"].Errors[0].ErrorMessage);
        }

        [Fact]
        public void Ensures_NonNull_Value_Is_In_ModelState_For_Each_Key()
        {
            // Arrange
            var ex = new RulesException("myProp", "myError");
            var modelState = new ModelStateDictionary();

            // Act
            ex.AddModelStateErrors(modelState, null);

            // Assert
            Assert.Equal(1, modelState.Keys.Count());
            Assert.NotNull(modelState["myProp"].Value);
        }

        [Fact]
        public void Does_Not_Overwrite_Any_Existing_ModelState_Value()
        {
            // Arrange
            object rawValue = new object();
            var someValue = new ModelState { Value = new ValueProviderResult(rawValue, null, null)};
            var ex = new RulesException("myProp", "myError");
            var modelState = new ModelStateDictionary();
            modelState.Add("myProp", someValue);

            // Act
            ex.AddModelStateErrors(modelState, null);

            // Assert
            Assert.Equal(1, modelState.Keys.Count());
            Assert.Same(rawValue, modelState["myProp"].Value.RawValue);
        }
    }
}