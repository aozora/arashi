 #region Disclaimer/Info
 
 /////////////////////////////////////////////////////////////////////////////////////////////////
 //
 //   File:		MockCreatedEventArgs.cs
 //   Website:		http://dexterblogengine.com/
 //   Authors:		http://dexterblogengine.com/About.ashx
 //   Rev:		1
 //   Created:		22/11/2010
 //   Last edit:		19/01/2011
 //   License:		GNU Library General Public License (LGPL)
 //   File:            MockCreatedEventArgs.cs
 //   For updated news and information please visit http://dexterblogengine.com/
 //   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
 //   For any question contact info@dexterblogengine.com
 //
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 
 #endregion
 
using System;

namespace Dexter.TestBase.AutoMockingContainer {
	public class MockCreatedEventArgs : EventArgs {
		public MockCreatedEventArgs ( object mock , string dependencyName ) {
			Mock = mock;
			DependencyName = dependencyName;
		}

		public Object Mock { get; set; }
		public String DependencyName { get; set; }

		public T Get <T> ( ) {
			return ( T ) Mock;
		}
	}
}
