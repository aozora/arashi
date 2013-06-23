namespace TestBase
{
   #region Usings

   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using MbUnit.Framework;
   using Arashi.Core.Extensions;

   #endregion

   public abstract class BaseTestFixtureWithHelper : BaseTestFixture
   {
      private static readonly Dictionary<Type, List<ITestHelper>> helpersMap = new Dictionary<Type, List<ITestHelper>>();
      protected List<ITestHelper> Helpers = new List<ITestHelper>();
      protected List<ITestHelper> TestLocalHelpers = new List<ITestHelper>();


      protected BaseTestFixtureWithHelper()
      {
         if (!helpersMap.ContainsKey(GetType()))
            Assert.Fail("You forget to call ClassInit() method on the test class in the Class initialize method");
         Helpers = helpersMap[GetType()];
      }



      protected override void OnSetUp()
      {
         object theContext = this.GetPropertyValue<Object>("TestContext");
         IDictionary contextProperties = theContext.GetPropertyValue<IDictionary>("Properties");
         String testname = (String)contextProperties["TestName"];
         MethodInfo mi = GetType().GetMethod(testname, BindingFlags.Instance | BindingFlags.Public);
         object[] attributes = mi.GetCustomAttributes(true);
         
         foreach (ITestHelperAttribute testHelperAttribute in attributes.OfType<ITestHelperAttribute>())
         {
            TestLocalHelpers.Add(testHelperAttribute.Create());
         }

         foreach (ITestHelper helper in TestLocalHelpers.Union(Helpers))
            helper.SetUp(this);

         base.OnSetUp();
      }



      protected override void OnTearDown()
      {
         foreach (ITestHelper helper in TestLocalHelpers.Union(Helpers).Reverse())
            helper.TearDown(this);
         base.OnTearDown();
      }



      protected static void ClassInit(Type type)
      {
         List<ITestHelper> helpers = new List<ITestHelper>();
         helpersMap[type] = helpers;
         Attribute[] attributes = Attribute.GetCustomAttributes(type);

         foreach (ITestHelperAttribute attribute in attributes.OfType<ITestHelperAttribute>())
         {
            ITestHelper helper = attribute.Create();
            helpers.Add(helper);
            helper.FixtureSetUp();
         }

         SetUpFixture(type);
      }



      protected static void ClassCleanup(Type type)
      {
         foreach (ITestHelper helper in helpersMap[type])
         {
            helper.FixtureTearDown();
         }
         helpersMap.Remove(type);
         TearDownFixture(type);
      }


   }
}