using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;

namespace Arashi.Web.Widgets
{
   public interface IWidgetComponent
   {
      /// <summary>
      /// A widget instance with all the instance settings
      /// </summary>
      Widget Widget {get;set;}



      /// <summary>
      /// The current RequestContext with info about current & managed site and current user
      /// </summary>
      IRequestContext Context {set;}



      /// <summary>
      /// Widget Component Initialization
      /// </summary>
      void Init();



      /// <summary>
      /// This method render the html for the widget
      /// </summary>
      /// <returns></returns>
      string Render();

   }
}
