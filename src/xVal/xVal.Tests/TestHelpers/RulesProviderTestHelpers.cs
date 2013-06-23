using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.Tests.TestHelpers
{
    public static class RulesProviderTestHelpers
    {
        public static TRule TestConversion<TAttribute, TRule>(IRulesProvider ruleProvider, params object[] attributeConstructorParams)
            where TAttribute : Attribute
            where TRule : Rule
        {
            var propertyRules = TestConversionToMultipleRules<TAttribute, TRule>(ruleProvider, attributeConstructorParams);
            Assert.Equal(1, propertyRules.Count());
            return propertyRules.Single();
        }

        public static IEnumerable<TRule> TestConversionToMultipleRules<TAttribute, TRule>(IRulesProvider ruleProvider, params object[] attributeConstructorParams)
            where TAttribute : Attribute
            where TRule : Rule
        {
            // Arrange
            Type testType = EmitTestType(typeof(TAttribute), attributeConstructorParams);

            // Act
            var allRules = ruleProvider.GetRulesFromType(testType);

            // Assert
            Assert.Equal(1, allRules.Keys.Count());
            var propertyRules = allRules["testProperty"];
            Assert.True(propertyRules.All(x => x is TRule));
            return propertyRules.Cast<TRule>();
        }

        /// <summary>
        /// Produces a .NET type with a property called testProperty that has a custom attribute
        /// </summary>
        /// <param name="attributeType">Type of the custom attribute to apply</param>
        /// <param name="attributeConstructorParams">Constructor parameters for the custom attribute</param>
        /// <returns>The finished Type object</returns>
        public static Type EmitTestType(Type attributeType, object[] attributeConstructorParams)
        {
            // Define the type and its property
            var assembly = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName("testAssembly"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assembly.DefineDynamicModule("testModule");
            var typeBuilder = moduleBuilder.DefineType("testType");
            var prop = typeBuilder.DefineProperty("testProperty", PropertyAttributes.None, typeof(string), null);

            // The property needs a "get" method
            var getMethod = typeBuilder.DefineMethod("get_testProperty", MethodAttributes.Public, typeof(string), null);
            var getMethodILBuilder = getMethod.GetILGenerator();
            getMethodILBuilder.Emit(OpCodes.Ret);
            prop.SetGetMethod(getMethod);

            // Apply the custom attribute to the property
            var attributeConstructorParamTypes = attributeConstructorParams.Select(x => x.GetType()).ToArray();
            var customAttributeBuilder = new CustomAttributeBuilder(attributeType.GetConstructor(attributeConstructorParamTypes), attributeConstructorParams);
            prop.SetCustomAttribute(customAttributeBuilder);

            return typeBuilder.CreateType();
        }
    }
}