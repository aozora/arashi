#region Disclaimer/Info

/////////////////////////////////////////////////////////////////////////////////////////////////
//
//   File:		IMockingStrategy.cs
//   Website:		http://dexterblogengine.com/
//   Authors:		http://dexterblogengine.com/About.ashx
//   Rev:		1
//   Created:		22/11/2010
//   Last edit:		19/01/2011
//   License:		GNU Library General Public License (LGPL)
//   File:            IMockingStrategy.cs
//   For updated news and information please visit http://dexterblogengine.com/
//   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
//   For any question contact info@dexterblogengine.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;

namespace Arashi.UnitTests.AutoMockingContainer
{
   public enum MockingStrategyType
   {
      Mock,
      Resolve,
      NoAction
   }

   public class MockingStrategy
   {
      public static readonly MockingStrategy Default = new MockingStrategy
      {
         Mock = MockingStrategyType.Mock
      };

      public static readonly MockingStrategy NoAction = new MockingStrategy
      {
         Mock = MockingStrategyType.NoAction
      };

      public static readonly MockingStrategy Resolve = new MockingStrategy
      {
         Mock = MockingStrategyType.Resolve
      };

      public MockingStrategy(object instance, MockingStrategyType mock)
      {
         Instance = instance;
         Mock = mock;
      }

      public MockingStrategy()
      {
         Mock = MockingStrategyType.Mock;
      }

      public Object Instance
      {
         get;
         set;
      }
      public MockingStrategyType Mock
      {
         get;
         set;
      }
   }
}
