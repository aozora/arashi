namespace TestBase
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using MbUnit.Framework;

   public abstract class BaseTestFixture
   {
      #region Initialization and private

      private static readonly Dictionary<Type, List<IDisposable>> fixtureDisposableList = new Dictionary<Type, List<IDisposable>>();
      private static readonly Dictionary<Type, List<Action>> fixtureTearDownActions = new Dictionary<Type, List<Action>>();
      private readonly List<IDisposable> singleTestDisposableList;
      private readonly List<Action> singleTestTearDownActions;



      protected BaseTestFixture()
      {
         singleTestDisposableList = new List<IDisposable>();
         singleTestTearDownActions = new List<Action>();
      }



      protected static void SetUpFixture(Type type)
      {
         fixtureDisposableList[type] = new List<IDisposable>();
         fixtureTearDownActions[type] = new List<Action>();
      }



      protected static void TearDownFixture(Type type)
      {
         Boolean ErrorOnDispose = false;
         fixtureDisposableList[type].ForEach(d =>
         {
            try
            {
               d.Dispose();
            }
            catch (Exception)
            {
               ErrorOnDispose = true;
            }
         });

         Boolean ErrorOnTearDownAction = false;
         fixtureTearDownActions[type].ForEach(a =>
         {
            try
            {
               a();
            }
            catch (Exception)
            {
               ErrorOnTearDownAction = true;
            }
         });

         Assert.IsTrue(ErrorOnDispose == false, "Some disposable object generates errors during Fixture Tear Down");
         Assert.IsTrue(ErrorOnTearDownAction == false, "Some tear down action generates errors during Fixture Tear Down");
      }



      [SetUp]
      public void SetUp()
      {
         singleTestDisposableList.Clear();
         singleTestTearDownActions.Clear();
         OnSetUp();
      }



      protected virtual void OnSetUp()
      {
      }



      [TearDown]
      public void TearDown()
      {
         Boolean ErrorOnDispose = false;
         singleTestDisposableList.ForEach(d =>
         {
            try
            {
               d.Dispose();
            }
            catch (Exception ex)
            {
               Console.Error.WriteLine(ex.Message);
               ErrorOnDispose = true;
            }
         });

         Boolean ErrorOnTearDownAction = false;
         singleTestTearDownActions.ForEach(a =>
         {
            try
            {
               a();
            }
            catch (Exception ex)
            {
               Console.Error.WriteLine(ex.Message);
               ErrorOnTearDownAction = true;
            }
         });

         Assert.IsTrue(ErrorOnDispose == false, "Some disposable object generates errors during Test Tear Down");
         Assert.IsTrue(ErrorOnTearDownAction == false, "Some tear down action generates errors during Test Tear Down");
         
         OnTearDown();
      }



      protected virtual void OnTearDown()
      {
         TestContext.Clear();
      }

      #endregion

      #region Cleanup management

      public void DisposeAtTheEndOfTest(IDisposable disposableObject)
      {
         singleTestDisposableList.Add(disposableObject);
      }



      public void DisposeAtTheEndOfFixture(IDisposable disposableObject)
      {
         fixtureDisposableList[GetType()].Add(disposableObject);
      }



      public void ExecuteAtTheEndOfTest(Action action)
      {
         singleTestTearDownActions.Add(action);
      }



      public void ExecuteAtTheEndOfFixture(Action action)
      {
         fixtureTearDownActions[GetType()].Add(action);
      }

      #endregion

      #region Context

      private readonly Dictionary<String, object> TestContext = new Dictionary<string, object>();

      public void SetIntoTestContext(String key, Object value)
      {
         if (!TestContext.ContainsKey(key))
            TestContext.Add(key, value);
         else
            TestContext[key] = value;
      }



      public T GetFromTestContext<T>(String key)
      {
         return (T)TestContext[key];
      }



      public void RemoveFromTestContext(String key)
      {
         TestContext.Remove(key);
      }

      #endregion
   }
}
