using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Web.Testing.Light;

[WebTestClass]
public class AspNetNativeTests
{
    private const string TestPageUrl = "/Test?viewPath=/Tests/xVal.AspNetNative/AllPossibleRules.aspx";

    [WebTestMethod]
    public void RequiredField_Is_Required_And_Shows_Custom_Error_Message()
    {
        TestFieldValidation("myprefix_RequiredField", "", "something", "This is a custom error message for a required field");
    }

    [WebTestMethod]
    public void DataType_EmailAddress_Enforced()
    {
        TestFieldValidation("myprefix_DataType_EmailAddress_Field", "blah", "blah@domain.com", "DATATYPE_EMAIL");
    }

    [WebTestMethod]
    public void DataType_CreditCardLuhn_Enforced()
    {
        TestFieldValidation("myprefix_DataType_CreditCardLuhn_Field", "4111 11111111 1112", "4111-1111 11111111", "DATATYPE_CREDITCARDLUHN");
    }

    [WebTestMethod]
    public void DataType_Integer_Enforced()
    {
        TestFieldValidation("myprefix_DataType_Integer_Field", "32x", "-137", "DATATYPE_INTEGER");
    }

    [WebTestMethod]
    public void DataType_Decimal_Enforced()
    {
        TestFieldValidation("myprefix_DataType_Decimal_Field", "-32x3", "-323", "DATATYPE_DECIMAL");
        TestFieldValidation("myprefix_DataType_Decimal_Field", "32x3.442", "323.442", "DATATYPE_DECIMAL");
    }

    [WebTestMethod]
    public void DataType_Date_Enforced()
    {
        TestFieldValidation("myprefix_DataType_Date_Field", "05/0x9/2008", "05/09/2008", "DATATYPE_DATE");
    }

    [WebTestMethod]
    public void DataType_DateTime_Enforced()
    {
        TestFieldValidation("myprefix_DataType_DateTime_Field", "blah", "05/09/2008 3:44", "DATATYPE_DATETIME");
    }

    [WebTestMethod]
    public void DataType_Currency_Enforced()
    {
        TestFieldValidation("myprefix_DataType_Currency_Field", "£4509101.", "4509101", "DATATYPE_CURRENCY");
        TestFieldValidation("myprefix_DataType_Currency_Field", "4509101.381", "£ 4,509,101.38", "DATATYPE_CURRENCY");
    }

    [WebTestMethod]
    public void Regex_Enforced()
    {
        TestFieldValidation("myprefix_Regex_Field", "X12", "   B938_", "Enter a value of the form 'X123'");
        TestFieldValidation("myprefix_Regex_CaseInsensitive_Field", "AB1C", "...aUI", "Enter a value of the form 'aBc'");
    }

    [WebTestMethod]
    public void Range_Int_Enforced()
    {
        TestFieldValidation("myprefix_Range_Int_Field", "3", "6", "RANGE_NUMERIC_MINMAX:5,10");
    }

    [WebTestMethod]
    public void Range_Decimal_Enforced()
    {
        TestFieldValidation("myprefix_Range_Decimal_Field", "10.99", "10.98", "RANGE_NUMERIC_MAX:10.98");
    }

    [WebTestMethod]
    public void Range_String_Enforced()
    {
        TestFieldValidation("myprefix_Range_String_Field", "aardvarj", "aardvarl", "RANGE_STRING_MINMAX:aardvark,antelope");
    }

    [WebTestMethod]
    public void Range_DateTime_Min_Enforced()
    {
        TestFieldValidation("myprefix_Range_DateTime_Min_Field", "01/01/2000", "01/01/2002", "RANGE_DATETIME_MIN:2001-02-19 17:04:59");
    }

    [WebTestMethod]
    public void Range_DateTime_Max_Enforced()
    {
        TestFieldValidation("myprefix_Range_DateTime_Max_Field", "01/01/2002", "01/01/2000", "RANGE_DATETIME_MAX:2001-02-19 17:04:59");
    }

    [WebTestMethod]
    public void Range_DateTime_Range_Enforced()
    {
        TestFieldValidation("myprefix_Range_DateTime_Range_Field", "01/01/2000", "01/01/2002", "RANGE_DATETIME_MINMAX:2001-02-19 17:04:59,2003-04-15 06:20:25");
        TestFieldValidation("myprefix_Range_DateTime_Range_Field", "01/01/2005", "01/01/2002", "RANGE_DATETIME_MINMAX:2001-02-19 17:04:59,2003-04-15 06:20:25");
    }

    [WebTestMethod]
    public void StringLength_Min_Enforced()
    {
        TestFieldValidation("myprefix_StringLength_Min_Field", "ab", "abc", "STRINGLENGTH_MIN:3");
    }

    [WebTestMethod]
    public void StringLength_Max_Enforced()
    {
        TestFieldValidation("myprefix_StringLength_Max_Field", "abcdefg", "abcdef", "STRINGLENGTH_MAX:6");
    }

    [WebTestMethod]
    public void StringLength_Range_Enforced()
    {
        TestFieldValidation("myprefix_StringLength_Range_Field", "abc", "abcd", "STRINGLENGTH_MINMAX:4,7");
        TestFieldValidation("myprefix_StringLength_Range_Field", "abcdefgh", "abcdefg", "STRINGLENGTH_MINMAX:4,7");
    }

    [WebTestMethod]
    public void Comparison_Equals_Enforced()
    {
        TestFieldValidation("myprefix_Comparison_Equals", "bla", "blah", "COMPARISON_EQUALS:RequiredField",
            // Setup first: populate the RequiredField box first
            page => page.Elements.Find("myprefix_RequiredField").SetText("blah")
        );
    }

    [WebTestMethod]
    public void Comparison_DoesNotEqual_Enforced()
    {
        TestFieldValidation("myprefix_Comparison_DoesNotEqual", "blah", "blah2", "COMPARISON_DOESNOTEQUAL:RequiredField",
            // Setup first: populate the RequiredField box first
            page => page.Elements.Find("myprefix_RequiredField").SetText("blah")
        );
    }

    [WebTestMethod]
    public void Custom_Enforced()
    {
        TestFieldValidation("myprefix_Custom", "hey", "hello", "Please enter the string 'hello'");
    }

    private static void TestFieldValidation(string inputField, string invalidValue, string validValue, string expectedFailureMessage)
    {
        TestFieldValidation(inputField, invalidValue, validValue, expectedFailureMessage, null);
    }

    private static void TestFieldValidation(string inputField, string invalidValue, string validValue, string expectedFailureMessage, Action<HtmlPage> additionalSetup)
    {
        var page = new HtmlPage();
        page.Navigate(TestPageUrl);

        if (additionalSetup != null)
            additionalSetup(page);

        // Put in invalid value
        var inputControl = page.Elements.Find(inputField);
        inputControl.SetText(invalidValue, true);
        page.Elements.Find("submitButton").Click();

        // Check error message has appeared
        var errorMessage = inputControl.NextSibling;
        Assert.IsNotNull(errorMessage);
        Assert.AreEqual("span", errorMessage.TagName);
        Assert.IsTrue(errorMessage.IsVisible());
        Assert.AreEqual(expectedFailureMessage, errorMessage.GetInnerHtml());

        // Now put in a valid value
        inputControl.SetText(validValue, true);

        // Check error message is gone
        Assert.IsFalse(errorMessage.IsVisible());
    }
}
