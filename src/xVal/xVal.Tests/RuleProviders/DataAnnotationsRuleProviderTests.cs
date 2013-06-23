using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;
using Xunit;
using xVal.RuleProviders;
using xVal.Rules;
using xVal.Tests.TestHelpers;

namespace xVal.Tests.RuleProviders
{
    public class DataAnnotationsRuleProviderTests
    {
        [Fact]
        public void ImplementsIRuleProvider()
        {
            IRulesProvider instance = new DataAnnotationsRuleProvider();
        }

        [Fact]
        public void Constructor_Can_Accept_TypeDescriptionProvider_Factory()
        {
            new DataAnnotationsRuleProvider(x => new Moq.Mock<TypeDescriptionProvider>().Object);
        }

        [Fact]
        public void FindsValidationAttributesAttachedToPublicProperties()
        {
            // Arrange
            var provider = new DataAnnotationsRuleProvider();

            // Act
            var rules = provider.GetRulesFromType(typeof (TestModel));

            // Assert the right set of rules were found
            Assert.Equal(3, rules.Keys.Count());
            Assert.Equal(2, rules["PublicProperty"].Count());
            Assert.NotEmpty(rules["PublicProperty"].OfType<RequiredRule>());
            Assert.NotEmpty(rules["PublicProperty"].OfType<RangeRule>());

            // Check attributes properties were retained
            var stringLengthRule = (StringLengthRule)rules["ReadonlyProperty"].First();
            Assert.Equal(3, stringLengthRule.MaxLength);
            Assert.Equal("Too long", stringLengthRule.ErrorMessage);
            var emailAddressRule = (DataTypeRule)rules["PropertyWithLocalizedMessage"].First();
            Assert.Equal(DataTypeRule.DataType.EmailAddress, emailAddressRule.Type);
            Assert.Equal(typeof(TestResources), emailAddressRule.ErrorMessageResourceType);
            Assert.Equal("TestResourceItem", emailAddressRule.ErrorMessageResourceName);
        }



        [Fact]
        public void Uses_Metadata_Provider_To_Locate_Properties()
        {
            // Arrange a mock type descriptor instance plus a TypeDescriptionProvider that will return it 
            PropertyDescriptor prop1 = MakeMockPropertyDescriptor("myProp1", typeof(string), new RequiredAttribute());
            PropertyDescriptor prop2 = MakeMockPropertyDescriptor("myProp2", typeof(object), new RangeAttribute(3, 6), new DataTypeAttribute(DataType.EmailAddress));
            var mockTypeDescriptor = new Moq.Mock<ICustomTypeDescriptor>();
            mockTypeDescriptor.Expect(x => x.GetProperties())
                              .Returns(new PropertyDescriptorCollection(new[] { prop1, prop2 }));

            var mockTypeDescriptionProvider = new Moq.Mock<TypeDescriptionProvider>();
            mockTypeDescriptionProvider.Expect(x => x.GetTypeDescriptor(typeof(MyDummyType), null)).Returns(mockTypeDescriptor.Object);
            var ruleProvider = new DataAnnotationsRuleProvider(x => mockTypeDescriptionProvider.Object);

            // Act
            var rules = ruleProvider.GetRulesFromType(typeof (MyDummyType));

            // Assert
            Assert.Equal(2, rules.Keys.Count());
            Assert.IsType<RequiredRule>(rules["myProp1"].Single());
            Assert.Equal(2, rules["myProp2"].Count());
            Assert.IsType<RangeRule>(rules["myProp2"].First());
            Assert.IsType<DataTypeRule>(rules["myProp2"].Skip(1).First());
        }

        private static PropertyDescriptor MakeMockPropertyDescriptor(string propertyName, Type propertyType, params Attribute[] attributes)
        {
            var mockDescriptor = new Moq.Mock<PropertyDescriptor>("ignored", new Attribute[0]);
            mockDescriptor.Expect(x => x.Name).Returns(propertyName);
            mockDescriptor.Expect(x => x.PropertyType).Returns(propertyType);
            mockDescriptor.Expect(x => x.Attributes).Returns(new AttributeCollection(attributes));
            return mockDescriptor.Object;
        }

        private class MyDummyType {}

        [Fact]
        public void Converts_RequiredAttribute_To_RequiredRule()
        {
            TestConversion<RequiredAttribute, RequiredRule>();
        }

        [Fact] 
        public void Converts_StringLengthAttribute_To_StringLengthRule()
        {
            var rule = TestConversion<StringLengthAttribute, StringLengthRule>(5);
            Assert.Equal(5, rule.MaxLength);
            Assert.Null(rule.MinLength);
        }

        [Fact]
        public void Converts_RangeAttribute_Int_To_RangeRule()
        {
            var rule = TestConversion<RangeAttribute, RangeRule>((int)3, (int)6);
            Assert.Equal(3, Convert.ToInt32(rule.Min));
            Assert.Equal(6, Convert.ToInt32(rule.Max));
        }

        [Fact]
        public void Converts_RangeAttribute_Double_To_RangeRule()
        {
            var rule = TestConversion<RangeAttribute, RangeRule>((double)3.492, (double)6.32);
            Assert.Equal(3.492, Convert.ToDouble(rule.Min));
            Assert.Equal(6.32, Convert.ToDouble(rule.Max));
        }

        [Fact]
        public void Converts_RangeAttribute_String_To_RangeRule()
        {
            var rule = TestConversion<RangeAttribute, RangeRule>(typeof(string), "aaa", "zzz");
            Assert.Equal("aaa", Convert.ToString(rule.Min));
            Assert.Equal("zzz", Convert.ToString(rule.Max));
        }

        [Fact]
        public void Converts_RangeAttribute_DateTime_To_RangeRule()
        {
            var min = new DateTime(2003, 01, 23, 05, 03, 10);
            var max = new DateTime(2008, 06, 3);
            var rule = TestConversion<RangeAttribute, RangeRule>(typeof(DateTime), min.ToString(), max.ToString());
            Assert.Equal(min, Convert.ToDateTime(rule.Min));
            Assert.Equal(max, Convert.ToDateTime(rule.Max));    
        }

        [Fact]
        public void Converts_DataTypeAttribute_Email_To_DataTypeRule()
        {
            var rule = TestConversion<DataTypeAttribute, DataTypeRule>(DataType.EmailAddress);
            Assert.Equal(DataTypeRule.DataType.EmailAddress, rule.Type);
        }

        [Fact]
        public void Converts_DataTypeAttribute_DateTime_To_DataTypeRule()
        {
            var rule = TestConversion<DataTypeAttribute, DataTypeRule>(DataType.DateTime);
            Assert.Equal(DataTypeRule.DataType.DateTime, rule.Type);
        }

        [Fact]
        public void Converts_DataTypeAttribute_Date_To_DataTypeRule()
        {
            var rule = TestConversion<DataTypeAttribute, DataTypeRule>(DataType.Date);
            Assert.Equal(DataTypeRule.DataType.Date, rule.Type);
        }

        [Fact]
        public void Converts_DataTypeAttribute_Currency_To_DataTypeRule()
        {
            var rule = TestConversion<DataTypeAttribute, DataTypeRule>(DataType.Currency);
            Assert.Equal(DataTypeRule.DataType.Currency, rule.Type);
        }

        [Fact]
        public void Converts_DataTypeAttribute_Time_To_RegularExpressionRule()
        {
            var rule = TestConversion<DataTypeAttribute, RegularExpressionRule>(DataType.Time);
            Assert.Equal(RegularExpressionRule.Regex_Time, rule.Pattern);
            Assert.Equal(RegexOptions.IgnoreCase, rule.Options);
        }

        [Fact]
        public void Converts_DataTypeAttribute_Duration_To_RegularExpressionRule()
        {
            var rule = TestConversion<DataTypeAttribute, RegularExpressionRule>(DataType.Duration);
            Assert.Equal(RegularExpressionRule.Regex_Duration, rule.Pattern);
            Assert.Equal(RegexOptions.IgnoreCase, rule.Options);
        }

        [Fact]
        public void Converts_DataTypeAttribute_PhoneNumber_To_RegularExpressionRule()
        {
            var rule = TestConversion<DataTypeAttribute, RegularExpressionRule>(DataType.PhoneNumber);
            Assert.Equal(RegularExpressionRule.Regex_USPhoneNumber, rule.Pattern);
            Assert.Equal(RegexOptions.IgnoreCase, rule.Options);
        }

        [Fact]
        public void Converts_DataTypeAttribute_Url_To_RegularExpressionRule()
        {
            var rule = TestConversion<DataTypeAttribute, RegularExpressionRule>(DataType.Url);
            Assert.Equal(RegularExpressionRule.Regex_Url, rule.Pattern);
            Assert.Equal(RegexOptions.IgnoreCase, rule.Options);
        }

        [Fact]
        public void Ignores_Other_DataType()
        {
            // Arrange
            var provider = new DataAnnotationsRuleProvider();
            Type testType = RulesProviderTestHelpers.EmitTestType(typeof(DataTypeAttribute), new object[] { DataType.Custom });

            // Act
            var rules = provider.GetRulesFromType(testType);

            // Assert
            Assert.Empty(rules.Keys);
        }

        [Fact]
        public void Converts_RegularExpressionAttribute_To_RegularExpressionRule()
        {
            var rule = TestConversion<RegularExpressionAttribute, RegularExpressionRule>("somepattern");
            Assert.Equal("somepattern", rule.Pattern);
            Assert.Equal(RegexOptions.None, rule.Options);
        }

        [Fact]
        public void Adds_Rules_For_ValueType_Numeric_Properties()
        {
            // Arrange
            var provider = new DataAnnotationsRuleProvider();

            // Act
            var rules = provider.GetRulesFromType(typeof (ModelWithValueTypeProperties));

            // Assert
            Assert.Equal(4, rules.Keys.Count());
            Assert.Equal(DataTypeRule.DataType.Integer, rules["IntProperty"].OfType<DataTypeRule>().Single().Type);
            Assert.Equal(DataTypeRule.DataType.Decimal, rules["DoubleProperty"].OfType<DataTypeRule>().Single().Type);
            Assert.Equal(DataTypeRule.DataType.Decimal, rules["FloatProperty"].OfType<DataTypeRule>().Single().Type);
            Assert.Equal(DataTypeRule.DataType.Decimal, rules["DecimalProperty"].OfType<DataTypeRule>().Single().Type);
        }

        private static TRule TestConversion<TAttribute, TRule>(params object[] attributeConstructorParams)
            where TAttribute : ValidationAttribute
            where TRule : Rule
        {
            return RulesProviderTestHelpers.TestConversion<TAttribute, TRule>(new DataAnnotationsRuleProvider(), attributeConstructorParams);
        }

        private class TestModel
        {
            [Required]
            [Range(5, 10)]
            public object PublicProperty { get; set; }

            [StringLength(3, ErrorMessage = "Too long")]
            public object ReadonlyProperty { get; private set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof (TestResources), ErrorMessageResourceName = "TestResourceItem")]
            public object PropertyWithLocalizedMessage { get; set; }


            public object PropertyWithNoValidationAttributes { get; set; }

            [Required] // Shouldn't be detected (TypeDescriptor doesn't tell you about write-only properties)
            public object WriteOnlyProperty { private get; set; }

            [Required] // Shouldn't be detected
                private object PrivateProperty { get; set; }
        }

        private class ModelWithValueTypeProperties
        {
            public int? IntProperty { get; set; }
            public double DoubleProperty { get; set; }
            public float? FloatProperty { get; set; }
            public decimal DecimalProperty { get; set; }
            public string StringProperty { get; set; }
        }

        private class TestResources
        {
            public static string TestResourceItem { get; set; }
        }

        [Fact]
        public void Can_Detect_Attributes_On_Buddy_Class()
        {
            // Arrange
            var provider = new DataAnnotationsRuleProvider();

            // Act
            var rules = provider.GetRulesFromType(typeof (DummyGeneratedClass));

            // Assert
            Assert.Equal(2, rules.Keys.Count());
            Assert.Equal(2, rules["Name"].Count());
            Assert.Equal(2, rules["Age"].Count());
            Assert.Equal(1, rules["Name"].OfType<RequiredRule>().Count());
            Assert.Equal(1, rules["Name"].OfType<StringLengthRule>().Count());
            Assert.Equal(1, rules["Age"].OfType<DataTypeRule>().Count()); // Should detect this as integer (preferring type on real class over type on buddy class)
            Assert.Equal(1, rules["Age"].OfType<RangeRule>().Count());
        }

        [MetadataType(typeof(DummyBuddyClass))]
        private class DummyGeneratedClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class DummyBuddyClass
        {
            [Required] [StringLength(50)] public string Name { get; set; }
            [Range(0, 150)] public object Age { get; set; }
        }
    }
}