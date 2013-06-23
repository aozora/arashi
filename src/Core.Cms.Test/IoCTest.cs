using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Arashi.Core;
using Castle.MicroKernel;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using NUnit.Framework;

namespace Arashi.Core.Cms.Test
{
   [TestFixture]
   public class IoCTest
   {

      [Test]
      public void VerifyCastleWindsorMappings()
      {
         //string currentPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "services.config");
         //Debug.WriteLine(currentPath);

         //IWindsorContainer container = new WindsorContainer(currentPath);
         IWindsorContainer container = new WindsorContainer(new XmlInterpreter());
         IoC.Initialize(container);

         foreach (IHandler handler in container.Kernel.GetAssignableHandlers(typeof(object)))
         {

            container.Resolve(handler.ComponentModel.Service);

         }

      }


   }
}