using System;
using System.Collections.Generic;

namespace Arashi.Web.Mvc.Models
{
   /// <summary>
   /// This class give a controller the support for registering external javascript as needed
   /// </summary>
   public class ScriptModel
   {
      /// <summary>
      /// Get/Set a list of available javascript libraries
      /// </summary>
      public IDictionary<ScriptName, String> AvailableScripts {get; set;}

      /// <summary>
      /// Get/Set a list of registered script to be rendered in the View
      /// </summary>
      public IList<ScriptName> RegisteredScripts {get; set;}

      /// <summary>
      /// Get/Set a list of javascript block to be rendered in the View
      /// </summary>
      public IDictionary<String, String> RegisteredScriptBlocks
      {
         get;
         set;
      }


      public enum ScriptName
      {
         Validate,
         Form,
         Taconite,
         ScrollTo,
         TreeView,

         // UI
         Finder,
         DateRangePicker,
         SelectToUISlider
      }



      public ScriptModel()
      {
         RegisteredScripts = new List<ScriptName>();
         RegisteredScriptBlocks = new Dictionary<String, String>();

         AvailableScripts = new Dictionary<ScriptName, String>
                               {
                                  {ScriptName.Validate, "jquery.validate"},
                                  {ScriptName.Form, "jquery.form"},
                                  {ScriptName.Taconite, "jquery.taconite"},
                                  {ScriptName.ScrollTo, "jquery.scrollTo"},
                                  {ScriptName.TreeView, "jquery.treeview"},
                                  {ScriptName.Finder, "ui.finder"},
                                  {ScriptName.DateRangePicker, "fg.daterangepicker"},
                                  {ScriptName.SelectToUISlider, "fg.selectToUISlider"}
                               };
      }



      /// <summary>
      /// Add a script to the list of scripts that will be rendered in the view
      /// </summary>
      /// <param name="scriptName"></param>
      public void AddScript(ScriptName scriptName)
      {
         if (!RegisteredScripts.Contains(scriptName))
            RegisteredScripts.Add(scriptName);

      }



      /// <summary>
      /// Add a script block that will be rendered in the view
      /// </summary>
      /// <param name="name"></param>
      /// <param name="script"></param>
      public void AddScriptBlock(string name, string script)
      {
         if (!RegisteredScriptBlocks.ContainsKey(name))
            RegisteredScriptBlocks.Add(name, script);

      }

   }
}
