using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Castle.Windsor;
using Castle.Windsor.Configuration;
using Rhino.Mocks;

namespace Arashi.UnitTests.AutoMockingContainer
{
   public sealed class AutoMockingContainer : WindsorContainer, IAutoMockingRepository
   {
      readonly AutoMockingFacility autoMockingFacility;
      readonly Dictionary<Type, List<Object>> mocks = new Dictionary<Type, List<Object>>();

      readonly Dictionary<StrategyKey, MockingStrategy> strategies = new Dictionary<StrategyKey, MockingStrategy>();


      public AutoMockingContainer(IConfigurationInterpreter interpreter)
         : base(interpreter)
      {
         autoMockingFacility = new AutoMockingFacility(this);
         AddFacility("Automocking", autoMockingFacility);
         ResolveProperties = true;
         DependencyToIgnore = new List<string>();
      }


      public AutoMockingContainer()
      {
         autoMockingFacility = new AutoMockingFacility(this);
         AddFacility("Automocking", autoMockingFacility);
         ResolveProperties = true;
         DependencyToIgnore = new List<string>();
      }

      #region IAutoMockingRepository Members

      public void OnMockCreated(Type type, Object mock, String dependencyName)
      {
         if (!mocks.ContainsKey(type))
         {
            mocks.Add(type, new List<object>());
         }
         mocks[type].Add(mock);
         EventHandler<MockCreatedEventArgs> temp = MockCreated;
         if (temp != null)
         {
            temp(this, new MockCreatedEventArgs(mock, dependencyName));
         }
      }

      public void OnMockCreated<T>(Object mock, String dependencyName)
      {
         OnMockCreated(typeof(T), mock, dependencyName);
      }

      void IAutoMockingRepository.AddStrategy(Type serviceType, MockingStrategy strategy)
      {
         strategies[new StrategyKey(serviceType, String.Empty)] = strategy;
      }

      public MockingStrategy GetStrategyFor(DependencyModel model)
      {
         MockingStrategy strategy = strategies
            .Where(kvp => kvp.Key.IsValidFor(model))
            .Select(kvp => kvp.Value)
            .FirstOrDefault();
         return strategy ?? MockingStrategy.Default;
      }

      #endregion

      public event EventHandler<MockCreatedEventArgs> MockCreated;

      public void ClearAllStrategies()
      {
         strategies.Clear();
      }

      public void MarkNonMocked<T>()
      {
         MarkNonMocked(typeof(T));
      }

      public void SetStrategyForDependencyName(string p, MockingStrategy strategy)
      {
         strategies.Add(new StrategyKey(null, p), strategy);
      }

      public void MarkNonMocked(Type t)
      {
         IEnumerable<MockingStrategy> st = strategies
            .Where(kvp => kvp.Key.TypeKey == t)
            .Select(kvp => kvp.Value);

         foreach (MockingStrategy mockingStrategy in st)
         {
            mockingStrategy.Mock = MockingStrategyType.Resolve;
         }
      }

      public T GetFirstCreatedMock<T>()
      {
         if (mocks.ContainsKey(typeof(T)))
         {
            return (T)mocks[typeof(T)].FirstOrDefault();
         }

         foreach (var keyValuePair in mocks)
         {
            if (typeof(T).IsAssignableFrom(keyValuePair.Key))
            {
               return (T)keyValuePair.Value.First();
            }
         }
         return default(T);
      }

      public List<T> GetMock<T>()
      {
         return mocks[typeof(T)].Cast<T>().ToList();
      }

      #region Resolve mock

      public override T Resolve<T>(string key)
      {
         if (_instances.ContainsKey(typeof(T)))
         {
            return (T)_instances[typeof(T)];
         }
         else if (IsTypeRegistered(typeof(T)))
         {
            return base.Resolve<T>(key);
         }
         var mock = (T)MockRepository.GenerateStub(typeof(T));
         OnMockCreated<T>(mock, "");
         return mock;
      }

      public new T Resolve<T>()
      {
         if (_instances.ContainsKey(typeof(T)))
         {
            return (T)_instances[typeof(T)];
         }
         else if (IsTypeRegistered(typeof(T)))
         {
            return base.Resolve<T>();
         }
         var mock = (T)MockRepository.GenerateStub(typeof(T));
         OnMockCreated<T>(mock, "");
         return mock;
      }

      Boolean IsTypeRegistered(Type service)
      {
         return Kernel.GetHandler(service) != null;
      }

      #endregion

      #region Register instance

      readonly Dictionary<Type, Object> _instances = new Dictionary<Type, object>();

      public void RegisterInstance<T>(T instance)
      {
         RegisterInstance(typeof(T), instance);
      }

      public void RegisterInstance(Type serviceType, Object instance)
      {
         strategies[new StrategyKey(serviceType, String.Empty)] = new MockingStrategy
         {
            Instance = instance
         };
         _instances[serviceType] = instance;
      }

      #endregion

      #region Skipping properties

      /// <summary>
      /// These are the dependencies to ignore, sometimes for some object we need to automock
      /// not everything, expecially public properties, so we can setup an ignorelist.
      /// </summary>
      /// <value>The dependency to ignore.</value>
      public List<String> DependencyToIgnore
      {
         get;
         set;
      }

      /// <summary>
      /// If false the container will not populate properties with mock.
      /// </summary>
      /// <value><c>true</c> if [resolve properties]; otherwise, <c>false</c>.</value>
      public Boolean ResolveProperties
      {
         get;
         set;
      }

      /// <summary>
      /// Determines whether this instance [can satisfy dependency key] the specified dependency key.
      /// </summary>
      /// <param name="dependencyKey">The dependency key.</param>
      /// <returns>
      /// 	<c>true</c> if this instance [can satisfy dependency key] the specified dependency key; otherwise, <c>false</c>.
      /// </returns>
      public Boolean CanSatisfyDependencyKey(String dependencyKey)
      {
         return !DependencyToIgnore.Contains(dependencyKey);
      }

      #endregion
   }
}
