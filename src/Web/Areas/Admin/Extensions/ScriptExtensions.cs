using System.Text;
using System.Web;
using System.Web.Mvc;
using Arashi.Web.Areas.Admin.Models;
using Arashi.Web.Mvc.Models;
using log4net;

namespace Arashi.Web.Areas.Admin.Extensions
{
   public static class ScriptExtensions
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(ScriptExtensions));
      
      
      /// <summary>
      /// Render all the registered script libraries
      /// </summary>
      /// <param name="helper"></param>
      /// <param name="isDebug">If true, the helper renders the libraries in the \src folder</param>
      /// <returns></returns>
      public static string RenderRegisteredScript(this HtmlHelper helper, bool isDebug )
      {
         if (!helper.ViewContext.ViewData.ContainsKey("ScriptModel"))
         {
            log.Error("RenderRegisteredScript invoked but the ViewData[\"ScriptModel\"] doesn't exists!");
            return null;
         }

         //get the directory where the scripts are
         string scriptRoot = VirtualPathUtility.ToAbsolute("~/Resources/js");
         ScriptModel model = helper.ViewContext.ViewData["ScriptModel"] as ScriptModel;
         StringBuilder scripts = new StringBuilder();

         if (model == null)
         {
            log.Error("RenderRegisteredScript invoked but the ViewData[\"ScriptModel\"] is NULL!");
            return null;
         }


         if (isDebug)
            scriptRoot += "/src";

         foreach (ScriptModel.ScriptName name in model.RegisteredScripts)
         {
            scripts.AppendFormat("<script src=\"{0}/{1}{2}.js\" type=\"text/javascript\"></script>\r\n",
                                 scriptRoot,
                                 model.AvailableScripts[name],
                                 isDebug ? string.Empty : ".min"
                                 );
         }

         return scripts.ToString();
      }



      /// <summary>
      /// Render all the registered script blocks
      /// </summary>
      /// <param name="helper"></param>
      /// <returns></returns>
      public static string RenderRegisteredScriptBlocks(this HtmlHelper helper)
      {
         if (!helper.ViewContext.ViewData.ContainsKey("ScriptModel"))
         {
            log.Error("ScriptModel invoked but the ViewData[\"ScriptModel\"] doesn't exists!");
            return null;
         }

         //get the directory where the scripts are
         string scriptRoot = VirtualPathUtility.ToAbsolute("~/Resources/js");
         ScriptModel model = helper.ViewContext.ViewData["ScriptModel"] as ScriptModel;
         StringBuilder blocks = new StringBuilder();

         if (model == null)
         {
            log.Error("RenderRegisteredScript invoked but the ViewData[\"ScriptModel\"] is NULL!");
            return null;
         }


         foreach (string blockKey in model.RegisteredScriptBlocks.Keys )
         {
            blocks.Append("<script type=\"text/javascript\">\r\n");
            blocks.Append(model.RegisteredScriptBlocks[blockKey]);
            blocks.Append("\r\n</script>\r\n");
         }

         return blocks.ToString();
      }



   }
}
