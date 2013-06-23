using System.Text;
using log4net;

namespace Arashi.Web.Widgets.Meta
{
   public class MetaWidgetComponent : WidgetComponentBase
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(MetaWidgetComponent));

      #endregion

      #region Constructor

      public MetaWidgetComponent()
      {
      }

      #endregion

      public override void Init()
      {
         log.Debug("MetaWidgetComponent.Init: start");

         log.Debug("MetaWidgetComponent.Init: end");

         base.Init();
      }


      public override string Render()
      {
         StringBuilder html = new StringBuilder();
         html.AppendLine("<ul>");

         //<li><?php wp_loginout(); ?></li>--%>
         //<li><a href='<%= bloginfo("rss2_url") %>'>Entries (RSS)</a></li>
         //<li><a href='<%= bloginfo("atom_url") %>'>Entries (ATOM)</a></li>
         //<li><a href="http://validator.w3.org/check/referer" title="This page validates as XHTML 1.0 Transitional">Valid <abbr title="eXtensible HyperText Markup Language">XHTML</abbr></a></li>
         //<li><a href="http://gmpg.org/xfn/"><abbr title="XHTML Friends Network">XFN</abbr></a></li>
         //<%--<?php wp_meta(); ?>--%>

         html.AppendFormat("<li><a href=\"{0}\">Feed RSS</a></li>", GetCurrentSiteUrlRoot() + "/feed/");
         html.AppendFormat("<li><a href=\"{0}\">Feed ATOM</a></li>", GetCurrentSiteUrlRoot() + "/feed/atom/");

         //<%--<?php wp_meta(); ?>--%>

         html.AppendLine("</ul>");
         return html.ToString();
      }


   }
}
