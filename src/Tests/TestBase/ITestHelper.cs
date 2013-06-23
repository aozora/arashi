namespace TestBase
{
   /// <summary>
   /// 	A test helper is an object that can be used to interact
   /// 	with the test
   /// </summary>
   public interface ITestHelper
   {
      void FixtureSetUp();
      void SetUp(BaseTestFixture fixture);
      void TearDown(BaseTestFixture fixture);
      void FixtureTearDown();
   }



   public interface ITestHelperAttribute
   {
      ITestHelper Create();
   }

}