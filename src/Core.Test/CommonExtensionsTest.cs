using System;
using NUnit.Framework;
using Arashi.Core.Extensions;

namespace Core.Test
{
   [TestFixture]
   public class CommonExtensionsTest
   {
      [Test]
      public void WithoutEndSeparatorCharTest()
      {
         string actual = string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), new string[] {"this", "is", "a", "path"})
                                    + System.IO.Path.DirectorySeparatorChar.ToString();

         string expected = actual.WithoutEndSeparatorChar();

         Assert.That(expected, Is.Not.EqualTo(actual));
         Assert.That(expected.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()), Is.False);
      }

   }
}
