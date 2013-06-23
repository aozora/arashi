using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Arashi.Core.Util;
using Text=Arashi.Core.Util.Text;


namespace Core.Test
{
   [TestFixture]
   public class TextTest
   {
      [Test]
      public void TruncateTextTest()
      {
         string actual = "This is a test";
         string expected = "This i";

         Assert.That(Text.TruncateText(actual, 6), Is.EqualTo(expected));
         Assert.That(Text.TruncateText(actual, 6, true), Is.EqualTo(expected + "..."));
         Assert.That(Text.TruncateText(actual, 6, true, true), Is.EqualTo(expected + "s..."));
      }




      [Test]
      public void StripHTMLTest()
      {
         string actual = "<html><body>This is <strong>a<strong> test</body></html>";
         string expected = "This is a test";

         Assert.That(Text.StripHTML(actual), Is.EqualTo(expected));
      }




   }
}
