namespace Arashi.UnitTests
{
   using System;
   using System.Collections.Generic;
   using Arashi.Core;
   using Castle.Windsor.Configuration.Interpreters;
   using log4net.Config;
   using NUnit.Framework;



   public abstract class TestBase
   {
      static TestBase()
      {
         // remember to initialize Log4Net or logging won't work.
         XmlConfigurator.Configure();
      }



      #region Initialization and private

      List<IDisposable> fixtureDisposableList;
      List<Action> fixtureTearDownActions;
      List<IDisposable> singleTestDisposableList;
      List<Action> singleTestTearDownActions;

      [TestFixtureSetUp]
      public void TestFixtureSetUp()
      {
         singleTestDisposableList = new List<IDisposable>();
         singleTestTearDownActions = new List<Action>();
         fixtureDisposableList = new List<IDisposable>();
         fixtureTearDownActions = new List<Action>();

         try
         {
            OnTestFixtureSetUp();
            SetUpContainer();
         }
         catch (Exception ex)
         {
            Console.Error.WriteLine("Error during fixture setup {0}", ex);
            throw;
         }
      }

      protected virtual void OnTestFixtureSetUp()
      {
      }


      internal const String CONTAINER_KEY = "AutoMockingContainerHelper_container";

      public Type[] Types
      {
         get;
         set;
      }

      public string[] IgnoreDependencies
      {
         get;
         set;
      }

      public bool ResolveProperties
      {
         get;
         set;
      }

      public void SetUpContainer()
      {
         var container = new AutoMockingContainer.AutoMockingContainer(new XmlInterpreter());
         this.DisposeAtTheEndOfTest(container);
         this.SetIntoTestContext(CONTAINER_KEY, container);
         //fixture.DisposeAtTheEndOfTest(DexterContainer.OverrideEngine(new CastleDexterContainer(container)));
         //this.DisposeAtTheEndOfTest(new WindsorContainer(container, new XmlInterpreter()));

         IoC.Initialize(container);
         //foreach (Type type in this.Types)
         //{
         //   container.Register(Component
         //                           .For(type)
         //                           .ImplementedBy(type)
         //                           .LifeStyle.Transient);
         //}

         container.ResolveProperties = ResolveProperties;

         if (IgnoreDependencies != null)
         {
            foreach (var ignoreDependency in IgnoreDependencies)
            {
               container.DependencyToIgnore.Add(ignoreDependency);
            }
         }
      }




      [TestFixtureTearDown]
      public void TestFixtureTearDown()
      {
         Boolean errorOnDispose = false;
         fixtureDisposableList.ForEach(d =>
         {
            try
            {
               d.Dispose();
            }
            catch (Exception)
            {
               errorOnDispose = true;
            }
         });
         Boolean errorOnTearDownAction = false;
         fixtureTearDownActions.ForEach(a =>
         {
            try
            {
               a();
            }
            catch (Exception)
            {
               errorOnTearDownAction = true;
            }
         });
         Assert.That(errorOnDispose == false, "Some disposable object generates errors during Fixture Tear Down");
         Assert.That(errorOnTearDownAction == false, "Some tear down action generates errors during Fixture Tear Down");
         OnTestFixtureTearDown();
      }

      protected virtual void OnTestFixtureTearDown()
      {
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
         Boolean errorOnDispose = false;
         singleTestDisposableList.ForEach(d =>
         {
            try
            {
               d.Dispose();
            }
            catch (Exception ex)
            {
               Console.Error.WriteLine(ex.Message);
               errorOnDispose = true;
            }
         });
         Boolean errorOnTearDownAction = false;
         singleTestTearDownActions.ForEach(a =>
         {
            try
            {
               a();
            }
            catch (Exception ex)
            {
               Console.Error.WriteLine(ex.Message);
               errorOnTearDownAction = true;
            }
         });
         Assert.That(errorOnDispose == false, "Some disposable object generates errors during Test Tear Down");
         Assert.That(errorOnTearDownAction == false, "Some tear down action generates errors during Test Tear Down");
         OnTearDown();
      }

      protected virtual void OnTearDown()
      {
         testContext.Clear();
      }

      #endregion

      #region Cleanup management

      public void DisposeAtTheEndOfTest(IDisposable disposableObject)
      {
         singleTestDisposableList.Add(disposableObject);
      }

      public void DisposeAtTheEndOfFixture(IDisposable disposableObject)
      {
         fixtureDisposableList.Add(disposableObject);
      }

      public void ExecuteAtTheEndOfTest(Action action)
      {
         singleTestTearDownActions.Add(action);
      }

      public void ExecuteAtTheEndOfFixture(Action action)
      {
         fixtureTearDownActions.Add(action);
      }

      #endregion

      #region Context

      readonly Dictionary<String, object> testContext = new Dictionary<string, object>();

      public void SetIntoTestContext(String key, Object value)
      {
         if (!testContext.ContainsKey(key))
         {
            testContext.Add(key, value);
         }
         else
         {
            testContext[key] = value;
         }
      }

      public T GetFromTestContext<T>(String key)
      {
         if (testContext.ContainsKey(key))
            return (T)testContext[key];

         return default(T);
      }

      public void RemoveFromTestContext(string key)
      {
         if (testContext.ContainsKey(key))
         {
            testContext.Remove(key);
         }
      }

      #endregion

   }
}
