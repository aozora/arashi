namespace TestBase.AutoMockingContainer
{
	#region Usings

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using Castle.Core;
	using Castle.Facilities.FactorySupport;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using Rhino.Mocks;

	#endregion

	public class AutoMockingContainer : IAutoMockingRepository
	{
		private readonly Dictionary<StrategyKey, MockingStrategy> _strategies
			=new Dictionary<StrategyKey, MockingStrategy>();

		public AutoMockingContainer()
		{
			Container=new WindsorContainer();
			Container.AddFacility<FactorySupportFacility>();
			Container.AddFacility( "Automocking", new AutoMockingFacility( this ) );
		}

		private WindsorContainer Container { get; set; }

		#region IAutoMockingRepository Members

		void IAutoMockingRepository.AddStrategy( Type serviceType, MockingStrategy strategy )
		{
			_strategies[new StrategyKey( serviceType, String.Empty )]=strategy;
		}

		public MockingStrategy GetStrategyFor( DependencyModel model )
		{
			MockingStrategy strategy=_strategies
				.Where( kvp => kvp.Key.IsValidFor( model ) )
				.Select( kvp => kvp.Value )
				.FirstOrDefault();
			return strategy ?? MockingStrategy.Default;
		}

		public void AddChildContainer( IWindsorContainer childContainer )
		{
			Container.AddChildContainer( childContainer );
		}

		public IWindsorContainer AddComponent<I, T>( string key ) where T : class
		{
			return Container.AddComponent<I, T>( key );
		}

		public IWindsorContainer AddComponent<I, T>() where T : class
		{
			return Container.AddComponent<I, T>();
		}

		public IWindsorContainer AddComponent<T>( string key )
		{
			return Container.AddComponent<T>( key );
		}

		public IWindsorContainer AddComponent<T>()
		{
			return Container.AddComponent<T>();
		}

		public IWindsorContainer AddComponent( string key, Type serviceType, Type classType )
		{
			return Container.AddComponent( key, serviceType, classType );
		}

		public IWindsorContainer AddComponent( string key, Type classType )
		{
			return Container.AddComponent( key, classType );
		}

		public IWindsorContainer AddComponentLifeStyle<I, T>( string key, LifestyleType lifestyle ) where T : class
		{
			return Container.AddComponentLifeStyle<I, T>( key, lifestyle );
		}

		public IWindsorContainer AddComponentLifeStyle<I, T>( LifestyleType lifestyle ) where T : class
		{
			return Container.AddComponentLifeStyle<I, T>( lifestyle );
		}

		public IWindsorContainer AddComponentLifeStyle<T>( string key, LifestyleType lifestyle )
		{
			return Container.AddComponentLifeStyle<T>( key, lifestyle );
		}

		public IWindsorContainer AddComponentLifeStyle<T>( LifestyleType lifestyle )
		{
			return Container.AddComponentLifeStyle<T>( lifestyle );
		}

		public IWindsorContainer AddComponentLifeStyle( string key, Type serviceType, Type classType, LifestyleType lifestyle )
		{
			return Container.AddComponentLifeStyle( key, serviceType, classType, lifestyle );
		}

		public IWindsorContainer AddComponentLifeStyle( string key, Type classType, LifestyleType lifestyle )
		{
			return Container.AddComponentLifeStyle( key, classType, lifestyle );
		}

		public IWindsorContainer AddComponentProperties<I, T>( string key, IDictionary extendedProperties ) where T : class
		{
			return Container.AddComponentProperties<I, T>( key, extendedProperties );
		}

		public IWindsorContainer AddComponentProperties<I, T>( IDictionary extendedProperties ) where T : class
		{
			return Container.AddComponentProperties<I, T>( extendedProperties );
		}

		public IWindsorContainer AddComponentWithProperties<T>( string key, IDictionary extendedProperties )
		{
			return Container.AddComponentWithProperties<T>( key, extendedProperties );
		}

		public IWindsorContainer AddComponentWithProperties<T>( IDictionary extendedProperties )
		{
			return Container.AddComponentWithProperties<T>( extendedProperties );
		}

		public IWindsorContainer AddComponentWithProperties( string key, Type serviceType, Type classType,
		                                                     IDictionary extendedProperties )
		{
			return Container.AddComponentWithProperties( key, serviceType, classType, extendedProperties );
		}

		public IWindsorContainer AddComponentWithProperties( string key, Type classType, IDictionary extendedProperties )
		{
			return Container.AddComponentWithProperties( key, classType, extendedProperties );
		}

		public IWindsorContainer AddFacility<T>( Func<T, object> onCreate ) where T : IFacility, new()
		{
			return Container.AddFacility( onCreate );
		}

		public IWindsorContainer AddFacility<T>( Action<T> onCreate ) where T : IFacility, new()
		{
			return Container.AddFacility( onCreate );
		}

		public IWindsorContainer AddFacility<T>() where T : IFacility, new()
		{
			return Container.AddFacility<T>();
		}

		public IWindsorContainer AddFacility<T>( string key, Func<T, object> onCreate ) where T : IFacility, new()
		{
			return Container.AddFacility( key, onCreate );
		}

		public IWindsorContainer AddFacility<T>( string key, Action<T> onCreate ) where T : IFacility, new()
		{
			return Container.AddFacility( key, onCreate );
		}

		public IWindsorContainer AddFacility<T>( string key ) where T : IFacility, new()
		{
			return Container.AddFacility<T>( key );
		}

		public IWindsorContainer AddFacility( string key, IFacility facility )
		{
			return Container.AddFacility( key, facility );
		}

		public IWindsorContainer GetChildContainer( string name )
		{
			return Container.GetChildContainer( name );
		}

		public IWindsorContainer Install( params IWindsorInstaller[] installers )
		{
			return Container.Install( installers );
		}

		public IKernel Kernel
		{
			get { return Container.Kernel; }
		}

		public string Name
		{
			get { return Container.Name; }
		}

		public IWindsorContainer Parent
		{
			get { return Container.Parent; }
			set { Container.Parent=value; }
		}

		public IWindsorContainer Register( params IRegistration[] registrations )
		{
			return Container.Register( registrations );
		}

		public void Release( object instance )
		{
			Container.Release( instance );
		}

		public void RemoveChildContainer( IWindsorContainer childContainer )
		{
			Container.RemoveChildContainer( childContainer );
		}

		public object Resolve( string key, Type service, object argumentsAsAnonymousType )
		{
			return Container.Resolve( key, service, argumentsAsAnonymousType );
		}

		public object Resolve( string key, Type service, IDictionary arguments )
		{
			return Container.Resolve( key, service, arguments );
		}

		public T Resolve<T>( string key, object argumentsAsAnonymousType )
		{
			return Container.Resolve<T>( key, argumentsAsAnonymousType );
		}

		public T Resolve<T>( string key, IDictionary arguments )
		{
			return Container.Resolve<T>( key, arguments );
		}

		public T Resolve<T>( string key )
		{
			return Container.Resolve<T>( key );
		}

		public T Resolve<T>( object argumentsAsAnonymousType )
		{
			return Container.Resolve<T>( argumentsAsAnonymousType );
		}

		public T Resolve<T>( IDictionary arguments )
		{
			return Container.Resolve<T>( arguments );
		}

		public T Resolve<T>()
		{
			if ( !Container.Kernel.HasComponent( typeof (T) ) )
				Container.Register(
					Component.For<T>()
						.LifeStyle.Transient
						.UsingFactoryMethod( () => (T) MockRepository.GenerateStub( typeof (T) ) ) );
			return Container.Resolve<T>();
		}

		public object Resolve( Type service, object argumentsAsAnonymousType )
		{
			return Container.Resolve( service, argumentsAsAnonymousType );
		}

		public object Resolve( Type service, IDictionary arguments )
		{
			return Container.Resolve( service, arguments );
		}

		public object Resolve( Type service )
		{
			return Container.Resolve( service );
		}

		public object Resolve( string key, Type service )
		{
			return Container.Resolve( key, service );
		}

		public object Resolve( string key, object argumentsAsAnonymousType )
		{
			return Container.Resolve( key, argumentsAsAnonymousType );
		}

		public object Resolve( string key, IDictionary arguments )
		{
			return Container.Resolve( key, arguments );
		}

		public object Resolve( string key )
		{
			return Container.Resolve( key );
		}

		public T[] ResolveAll<T>( object argumentsAsAnonymousType )
		{
			return Container.ResolveAll<T>( argumentsAsAnonymousType );
		}

		public T[] ResolveAll<T>( IDictionary arguments )
		{
			return Container.ResolveAll<T>( arguments );
		}

		public Array ResolveAll( Type service, object argumentsAsAnonymousType )
		{
			return Container.ResolveAll( service, argumentsAsAnonymousType );
		}

		public Array ResolveAll( Type service, IDictionary arguments )
		{
			return Container.ResolveAll( service, arguments );
		}

		public Array ResolveAll( Type service )
		{
			return Container.ResolveAll( service );
		}

		public T[] ResolveAll<T>()
		{
			return Container.ResolveAll<T>();
		}

		public object this[ Type service ]
		{
			get { return Container[service]; }
		}

		public object this[ string key ]
		{
			get { return Container[key]; }
		}

		public T GetService<T>() where T : class
		{
			return Container.GetService<T>();
		}

		public object GetService( Type serviceType )
		{
			return Container.GetService( serviceType );
		}

		public void Dispose()
		{
			Container.Dispose();
		}

		#endregion

		public void ClearAllStrategies()
		{
			_strategies.Clear();
		}

		public void MarkNonMocked<T>()
		{
			MarkNonMocked( typeof (T) );
		}

		public void SetStrategyForDependencyName( string p, MockingStrategy strategy )
		{
			_strategies.Add( new StrategyKey( null, p ), strategy );
		}

		public void MarkNonMocked( Type t )
		{
			_strategies
				.Where( kvp => kvp.Key.TypeKey == t )
				.Select( kvp => kvp.Value )
				.ToList()
				.ForEach( s => s.Mock=MockingStrategyType.Resolve );
		}

		public void RegisterInstance<T>( T instance )
		{
			RegisterInstance( typeof (T), instance );
		}


		public void RegisterInstance( Type serviceType, Object instance )
		{
			_strategies[new StrategyKey( serviceType, String.Empty )]=new MockingStrategy {Instance=instance};
		}
	}
}