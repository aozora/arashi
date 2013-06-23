using System;
using NUnit.Framework;
using Arashi.Core.Extensions;

namespace Core.Test
{
   [TestFixture]
   public class Base64ExtensionsTest
   {

      [Test]
      public void EncodingTest()
      {
         string actual = "this is a test";
         string encoded = actual.EncodeToBase64();

         Assert.That(encoded, Is.Not.Empty);
         Assert.That(encoded, Is.Not.Null);
      }



      [Test]
      public void EncodingDecodingTest()
      {
         string actual = "this is a test";
         string encoded = actual.EncodeToBase64();
         string decoded = encoded.DecodeFromBase64();

         // note: the case must be the same
         Assert.That(decoded, Is.EqualTo(actual));
      }

   }
}
