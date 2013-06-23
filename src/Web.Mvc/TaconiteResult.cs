using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Arashi.Web.Mvc
{
   /// <summary>
   /// ActionResult for use with the client side library jQuery Taconite
   /// </summary>
   public class TaconiteResult : ActionResult
   {
      private StringBuilder template;

      public TaconiteResult()
      {
         template = new StringBuilder();
      }



      /// <summary>
      /// The taconite template
      /// </summary>
      public string Template
      {
         get
         {
            template.Insert(0, "<taconite>");
            template.Append("</taconite>");
            return template.ToString();
         }
      }


      /// <summary>
      /// Set the Taconite command Val
      /// </summary>
      /// <param name="select"></param>
      /// <param name="value"></param>
      public void SetVal(string select, string value)
      {
         template.AppendFormat("<val select=\"{0}\" value=\"{1}\" />", select, value);
      }



      /// <summary>
      /// Set the Taconite command Attr
      /// </summary>
      /// <param name="select"></param>
      /// <param name="name"></param>
      /// <param name="value"></param>
      public void SetAttr(string select, string name, string value)
      {
         template.AppendFormat("<attr select=\"{0}\" name=\"{1}\" value=\"{2}\" />", select, name, value);
      }



      /// <summary>
      /// Set the Taconite command ReplaceContent
      /// </summary>
      /// <param name="select"></param>
      /// <param name="content"></param>
      public void SetReplaceContent(string select, string content)
      {
         template.AppendFormat("<replaceContent select=\"{0}\"><![CDATA[{1}]]></replaceContent>", select, content);
      }



      /// <summary>
      /// Set the Taconite command Eval
      /// </summary>
      /// <param name="eval"></param>
      public void SetReplaceContent(string eval)
      {
         template.AppendFormat("<eval><![CDATA[{0}]]></eval>", eval);
      }



      #region Overrides of ActionResult

      /// <summary>
      /// Enables processing of the result of an action method by a custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
      /// </summary>
      /// <param name="context">The context within which the result is executed.</param>
      public override void ExecuteResult(ControllerContext context)
      {
         if (context == null)
            throw new ArgumentNullException("context");

         HttpResponseBase response = context.HttpContext.Response;
         response.ContentType = "text/xml";

         if (this.Template != null)
         {
            response.Write(this.Template);
         }
      }

      #endregion
   }
}
