﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Linq;
using ClientDependency.Core.Config;
using System.Web;

namespace ClientDependency.Core.FileRegistration.Providers
{
    public class PageHeaderProvider : WebFormsFileRegistrationProvider
	{		

		public const string DefaultName = "PageHeaderProvider";
		

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{			
			// Assign the provider a default name if it doesn't have one
			if (string.IsNullOrEmpty(name))
				name = DefaultName;

			base.Initialize(name, config);
		}

        protected override string RenderJsDependencies(List<IClientDependencyFile> jsDependencies, HttpContextBase http)
		{
			if (jsDependencies.Count == 0)
				return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
			{
				foreach (var dependency in jsDependencies)
				{
                    sb.Append(RenderSingleJsFile(dependency.FilePath));
				}
			}
			else
			{
                var comp = ProcessCompositeList(jsDependencies, ClientDependencyType.Javascript, http);
                foreach (var s in comp)
                {
                    sb.Append(RenderSingleJsFile(s));
                }    
			}

            return sb.ToString();
		}

		protected override string RenderSingleJsFile(string js)
		{
            return string.Format(HtmlEmbedContants.ScriptEmbedWithSource, js);
		}

        protected override string RenderCssDependencies(List<IClientDependencyFile> cssDependencies, HttpContextBase http)
		{
            if (cssDependencies.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();

            if (http.IsDebuggingEnabled || !EnableCompositeFiles)
			{
				foreach (var dependency in cssDependencies)
				{
                    sb.Append(RenderSingleCssFile(dependency.FilePath));
				}
			}
			else
			{
                var comp = ProcessCompositeList(cssDependencies, ClientDependencyType.Css, http);
                foreach (var s in comp)
                {
                    sb.Append(RenderSingleCssFile(s));
                }    
			}

            return sb.ToString();
		}

		protected override string RenderSingleCssFile(string css)
		{
            return string.Format(HtmlEmbedContants.CssEmbedWithSource, css);
		}

        /// <summary>
        /// Registers the dependencies in the page header
        /// </summary>
        /// <param name="http"></param>
        /// <param name="js"></param>
        /// <param name="css"></param>
        /// <remarks>
        /// For some reason ampersands that aren't html escaped are not compliant to HTML standards when they exist in 'link' or 'script' tags in URLs,
        /// we need to replace the ampersands with &amp; . This is only required for this one w3c compliancy, the URL itself is a valid URL.
        /// 
        /// </remarks>
        protected override void RegisterDependencies(HttpContextBase http, string js, string css)
        {
            if (!(http.CurrentHandler is Page))
            {
                throw new InvalidOperationException("The current HttpHandler in a WebFormsFileRegistrationProvider must be of type Page");
            }
            var page = (Page) http.CurrentHandler;

            if (page.Header == null)
                throw new NullReferenceException("PageHeaderProvider requires a runat='server' tag in the page's header tag");

            var jsScriptBlock = new LiteralControl(js.Replace("&", "&amp;"));
            var cssStyleBlock = new LiteralControl(css.Replace("&", "&amp;"));
            page.Header.Controls.Add(cssStyleBlock);
            page.Header.Controls.Add(jsScriptBlock);
        }
	}
}
