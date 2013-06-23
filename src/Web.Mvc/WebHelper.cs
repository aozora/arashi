using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;

namespace Arashi.Web.Helpers
{
   /// <summary>
   /// Summary description for Utils
   /// </summary>
   public static class WebHelper
   {
      #region Site Utilities

      /// <summary>
      /// Get the host name and port number
      /// </summary>
      /// <param name="protocol"></param>
      /// <returns></returns>
      private static string GetHost(string protocol)
      {
         string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
         string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

         // Most proxies add an X-Forwarded-Host header which contains the original Host header
         // including any non-default port.
         string forwardedHosts = HttpContext.Current.Request.Headers["X-Forwarded-Host"];

         if (forwardedHosts != null)
         {
            // If the request passed through multiple proxies, they will be separated by commas.
            // We only care about the first one.
            string forwardedHost = forwardedHosts.Split(',')[0];
            string[] serverAndPort = forwardedHost.Split(':');

            serverName = serverAndPort[0];
            serverPort = null;

            if (serverAndPort.Length > 1)
               serverPort = serverAndPort[1];
         }

         // Only include a port if it is not the default for the protocol and MapAlternatePort = true
         // in the config file.
         // TODO: usare MapAlternatePort
         if ((protocol == "http" && serverPort == "80") || (protocol == "https" && serverPort == "443"))
            serverPort = null;

         string host = serverName;

         if (serverPort != null)
         {
            host += ":" + serverPort;
         }
         return host;
      }



      /// <summary>
      /// Ottiene il percorso root dell'applicazione 
      /// (wrapper per HttpContext.Current.Request.ApplicationPath)
      /// </summary>
      /// <returns></returns>
      public static string GetApplicationRoot()
      {
         string path = HttpContext.Current.Request.ApplicationPath;

         if (path.Length == 1)
            return "";
         else
            return path;
      }



      /// <summary>
      /// Get the site root with port number (i.e. http://localhost:41)
      /// </summary>
      /// <returns></returns>
      public static string GetSiteRoot()
      {
         string protocol = "http";

         if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
            protocol += "s";

         string host = GetHost(protocol);

         return protocol + "://" + host + GetApplicationRoot();
      }



      /// <summary>
      /// Get the current host name with the port if specified in the uri
      /// </summary>
      /// <returns></returns>
      public static string GetHostName()
      {
         string protocol = "http";

         if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
            protocol += "s";

         return GetHost(protocol);
      }



      /// <summary>
      /// Get the secure site root
      /// </summary>
      /// <returns></returns>
      public static string GetSecureSiteRoot()
      {
         string protocol = "https";
         string host = GetHost(protocol);

         return protocol + "://" + host + GetApplicationRoot();
      }



      /// <summary>
      /// Get the virtual root
      /// </summary>
      /// <returns></returns>
      public static string GetVirtualRoot()
      {
         string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];

         return string.Concat("/", serverName, GetApplicationRoot());
      }



      ///// <summary>
      ///// Se l'url corrente non è su una connessione protetta 
      ///// reindirizzo il client sullo stesso url ma con https!
      ///// </summary>
      //public static void ForceSSL()
      //{
      //   //bool proxyPreventsSSLDetection = MasterSettingsBiz.GetSettings().ProxyPreventsSslDetection;

      //   //if (!proxyPreventsSSLDetection)
      //   //{
      //   string url = HttpContext.Current.Request.Url.ToString();
      //   if (url.StartsWith("http:"))
      //      HttpContext.Current.Response.Redirect("https" + url.Remove(0, 4), false);
      //   //}

      //}



      ///// <summary>
      ///// Forza l'url su connessione NON protetta
      ///// </summary>
      //public static void ClearSSL()
      //{
      //   string url = HttpContext.Current.Request.Url.ToString();

      //   if (url.IndexOf("https") > -1)
      //   {
      //      HttpContext.Current.Response.Redirect(url.Replace("https", "http"), false);
      //   }
      //}

      #endregion

      //#region Url Helpers

      ///// <summary>
      ///// Restituisce l'url della pagina di login per il Site della richiesta corrente
      ///// (usa HttpContext.Current.Request.Url)
      ///// </summary>
      ///// <returns></returns>
      //public static string GetLoginUrl()
      //{
      //   UrlBuilder loginUrl = new UrlBuilder(HttpContext.Current.Request.Url);
      //   loginUrl.QueryString.Clear();
      //   loginUrl.Path = System.Web.Security.FormsAuthentication.LoginUrl;

      //   return loginUrl.ToString();
      //}



      ///// <summary>
      ///// Restituisce l'url per un dato host nel formato "hostname/" (i.e. "www.pippo.com/")
      ///// Imposta correttamente anche la porta.
      ///// </summary>
      ///// <param name="hostName"></param>
      ///// <returns></returns>
      //public static string GetSiteUrl(string hostName)
      //{
      //   string[] host = hostName.Split(':');
      //   UrlBuilder siteUrl = new UrlBuilder {Host = host[0]};
      //   if (host.Length > 1)
      //      siteUrl.Port = Convert.ToInt32(host[1]);
      //   siteUrl.Path = "/";

      //   return siteUrl.ToString();
      //}




      //#endregion

      #region WebUtilities

      ///// <summary>
      ///// Controlla che un indirizzo di email sia formalmente corretto
      ///// </summary>
      ///// <param name="emailAddress"></param>
      ///// <returns>True se l'indirizzo è corretto</returns>
      //public static bool IsValidEmailAddressSyntax(string emailAddress)
      //{
      //   Regex emailPattern;
      //   emailPattern = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$");

      //   Match emailAddressToValidate = emailPattern.Match(emailAddress);

      //   if (emailAddressToValidate.Success)
      //      return true;
      //   else
      //      return false;

      //}



      ///// <summary>
      ///// CreateRandomPassword
      ///// </summary>
      ///// <param name="length"></param>
      ///// <returns></returns>
      //public static string CreateRandomPassword(int length)
      //{
      //   if (length == 0)
      //      length = 7;

      //   char[] allowedChars = "abcdefgijkmnopqrstwxyzABCDEFGHJKLMNPQRSTWXYZ23456789*$-+?_&=!%{}/".ToCharArray();
      //   char[] passwordChars = new char[length];
      //   byte[] seedBytes = new byte[4];

      //   RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
      //   crypto.GetBytes(seedBytes);

      //   int seed = (seedBytes[0] & 0x7f) << 24 |
      //               seedBytes[1] << 16 |
      //               seedBytes[2] << 8 |
      //               seedBytes[3];

      //   Random random = new Random(seed);

      //   for (int i = 0; i < length; i++)
      //   {
      //      passwordChars[i] = allowedChars[random.Next(0, allowedChars.Length)];
      //   }

      //   return new string(passwordChars);

      //}



      ///// <summary>
      ///// Crypta un testo cn l'algoritmo MD5
      ///// </summary>
      ///// <param name="cleanString"></param>
      ///// <returns></returns>
      //public static string Encrypt(string cleanString)
      //{

      //   Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
      //   Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

      //   return BitConverter.ToString(hashedBytes);
      //}


      
      /// <summary>
      /// Searches control hierarchy from top down to find a control matching the passed in name
      /// </summary>
      /// <param name="objParent">Root control to begin searching</param>
      /// <param name="strControlName">Name of control to look for</param>
      /// <returns></returns>
      /// <remarks>
      /// This differs from FindControlRecursive in that it looks down the control hierarchy, whereas, the 
      /// FindControlRecursive starts at the passed in control and walks the tree up.  Therefore, this function is 
      /// more a expensive task.
      /// </remarks>
      public static Control FindControlRecursive(Control objParent, string strControlName)
      {
         Control objCtl = objParent.FindControl(strControlName);

         if (objCtl == null)
         {
            foreach (Control objChild in objParent.Controls)
            {
               if (objChild.HasControls())
                  objCtl = FindControlRecursive(objChild, strControlName);

               if (!(objCtl == null))
                  return objCtl;
            }
         }
         return objCtl;
      }


      #endregion

      #region Security Helpers

      //'''-----------------------------------------------------------------------------
      //''' <summary>
      //''' This function applies security filtering to the UserInput string.
      //''' </summary>
      //''' <param name="UserInput">This is the string to be filtered</param>
      //''' <param name="FilterType">Flags which designate the filters to be applied</param>
      //''' <returns>Filtered UserInput</returns>
      //''' <history>
      //''' 	[Joe Brinkman] 	8/15/2003	Created Bug #000120, #000121
      //''' </history>
      //'''-----------------------------------------------------------------------------
      //public string InputFilter(string UserInput, FilterFlag FilterType ) {
      //    if (string.IsNullOrEmpty(UserInput)) return "";

      //    string TempInput = UserInput;

      //    If (FilterType And FilterFlag.NoAngleBrackets) = FilterFlag.NoAngleBrackets Then
      //        Dim RemoveAngleBrackets As Boolean
      //        If Config.GetSetting("RemoveAngleBrackets") Is Nothing Then
      //            RemoveAngleBrackets = False
      //        Else
      //            RemoveAngleBrackets = Boolean.Parse(Config.GetSetting("RemoveAngleBrackets"))
      //        End If
      //        If RemoveAngleBrackets = True Then
      //            TempInput = FormatAngleBrackets(TempInput)
      //        End If
      //    End If

      //    If (FilterType And FilterFlag.NoSQL) = FilterFlag.NoSQL Then
      //        TempInput = FormatRemoveSQL(TempInput)
      //    Else
      //        If (FilterType And FilterFlag.NoMarkup) = FilterFlag.NoMarkup AndAlso IncludesMarkup(TempInput) Then
      //            TempInput = HttpUtility.HtmlEncode(TempInput)
      //        End If

      //        If (FilterType And FilterFlag.NoScripting) = FilterFlag.NoScripting Then
      //            TempInput = FormatDisableScripting(TempInput)
      //        End If

      //        If (FilterType And FilterFlag.MultiLine) = FilterFlag.MultiLine Then
      //            TempInput = FormatMultiLine(TempInput)
      //        End If
      //    End If

      //    return TempInput;
      //}

      //'''-----------------------------------------------------------------------------
      //''' <summary>
      //''' This filter removes CrLf characters and inserts br
      //''' </summary>
      //''' <param name="strInput">This is the string to be filtered</param>
      //''' <returns>Filtered UserInput</returns>
      //''' <remarks>
      //''' This is a private function that is used internally by the InputFilter function
      //''' </remarks>
      //''' <history>
      //''' 	[Joe Brinkman] 	8/15/2003	Created Bug #000120
      //''' </history>
      //'''-----------------------------------------------------------------------------
      //Private Function FormatMultiLine(ByVal strInput As String) As String
      //    Dim TempInput As String = strInput.Replace(ControlChars.Cr + ControlChars.Lf, "<br>")
      //    Return TempInput.Replace(ControlChars.Cr, "<br>")
      //End Function    'FormatMultiLine

      //'''-----------------------------------------------------------------------------
      //''' <summary>
      //''' This function uses Regex search strings to remove HTML tags which are 
      //''' targeted in Cross-site scripting (XSS) attacks.  This function will evolve
      //''' to provide more robust checking as additional holes are found.
      //''' </summary>
      //''' <param name="strInput">This is the string to be filtered</param>
      //''' <returns>Filtered UserInput</returns>
      //''' <remarks>
      //''' This is a private function that is used internally by the InputFilter function
      //''' </remarks>
      //''' <history>
      //''' 	[Joe Brinkman] 	8/15/2003	Created Bug #000120
      //'''     [cathal]        3/06/2007   Added check for encoded content
      //''' </history>
      //'''-----------------------------------------------------------------------------
      //Private Function FormatDisableScripting(ByVal strInput As String) As String
      //    Dim TempInput As String = strInput
      //    TempInput = FilterStrings(TempInput)

      //    Return TempInput
      //End Function

      //'''-----------------------------------------------------------------------------
      //''' <summary>
      //''' This function uses Regex search strings to remove HTML tags which are 
      //''' targeted in Cross-site scripting (XSS) attacks.  This function will evolve
      //''' to provide more robust checking as additional holes are found.
      //''' </summary>
      //''' <param name="strInput">This is the string to be filtered</param>
      //''' <returns>Filtered UserInput</returns>
      //''' <remarks>
      //''' This is a private function that is used internally by the FormatDisableScripting function
      //''' </remarks>
      //''' <history>
      //'''     [cathal]        3/06/2007   Created
      //''' </history>
      //'''-----------------------------------------------------------------------------
      //Private Function FilterStrings(ByVal strInput As String) As String
      //    'setup up list of search terms as items may be used twice
      //    Dim TempInput As String = strInput
      //    Dim listStrings As New List(Of String)
      //    listStrings.Add("<script[^>]*>.*?</script[^><]*>")
      //    listStrings.Add("<input[^>]*>.*?</input[^><]*>")
      //    listStrings.Add("<object[^>]*>.*?</object[^><]*>")
      //    listStrings.Add("<embed[^>]*>.*?</embed[^><]*>")
      //    listStrings.Add("<applet[^>]*>.*?</applet[^><]*>")
      //    listStrings.Add("<form[^>]*>.*?</form[^><]*>")
      //    listStrings.Add("<option[^>]*>.*?</option[^><]*>")
      //    listStrings.Add("<select[^>]*>.*?</select[^><]*>")
      //    listStrings.Add("<iframe[^>]*>.*?</iframe[^><]*>")
      //    listStrings.Add("<ilayer[^>]*>.*?</ilayer[^><]*>")
      //    listStrings.Add("<form[^>]*>")
      //    listStrings.Add("</form[^><]*>")
      //    listStrings.Add("javascript:")
      //    listStrings.Add("vbscript:")
      //    listStrings.Add("alert.*\(?'?""?'?""?\)?")

      //    Dim options As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Singleline
      //    Dim strReplacement As String = " "
      //    For Each s As String In listStrings
      //        TempInput = Regex.Replace(TempInput, s, strReplacement, options)
      //    Next

      //    'check if text contains encoded angle brackets, if it does it we decode it to check the plain text
      //    If TempInput.Contains("&gt;") = True And TempInput.Contains("&lt;") = True Then
      //        'text is encoded, so we check with an encoded version of the strings         
      //        For Each s As String In listStrings
      //            TempInput = Regex.Replace(TempInput, HttpContext.Current.Server.HtmlEncode(s), strReplacement, options)
      //        Next
      //    End If

      //    Return TempInput

      //End Function

      #endregion

      #region Default Folders Enum

      public enum DefaultFolders
      {
         images,
         files,
         templates
      }

      #endregion

      #region SpecialFolder inner class

      /// <summary>
      /// Contiene le proprietà per restituire i path di determinate
      /// cartelle web ad uso globale
      /// </summary>
      public static class SpecialFolder
      {
         #region Folder Globali

         public static Uri HtmlEditorBasePath
         {
            get
            {
               return new UriBuilder(string.Concat(GetSiteRoot(), "/Resources/fckeditor/")).Uri;
            }
         }
         
         /// <summary>
         /// Restituisce il path per la cartella di base di tutti i Sites
         /// Ad es. http://www.mercanteweb.it/Sites/
         /// </summary>
         public static Uri GlobalSitesPath
         {
            get
            {
               return new UriBuilder(string.Concat(GetSiteRoot(), "/Sites/")).Uri;
            }
         }


         /// <summary>
         /// Restituisce il path relativo per la cartella di base di tutti i Sites
         /// Ad es. ~/Sites/
         /// </summary>
         public static string GlobalSitesRelativePath
         {
            get
            {
               string path = GlobalSitesPath.ToString();
               return string.Concat("~", path.Substring(path.IndexOf(GetSiteRoot()) + GetSiteRoot().Length));
            }
         }


         /// <summary>
         /// Uri delle immagini ad uso globale.
         /// Ad es. http://www.mercanteweb.it/Resources/img/
         /// </summary>
         public static Uri GlobalImagesPath
         {
            get
            {
               return new UriBuilder(string.Concat(GetSiteRoot(), "/Resources/img/")).Uri;
            }
         }

         /// <summary>
         /// Path relativo delle immagini ad uso globale.
         /// Ad es. ~/Resources/img/
         /// </summary>
         public static string GlobalImagesRelativePath
         {
            get
            {
               string path = GlobalImagesPath.ToString();
               return string.Concat("~", path.Substring(path.IndexOf(GetSiteRoot()) + GetSiteRoot().Length));
            }
         }


         /// <summary>
         /// Uri della cartella generale dei templates globali
         /// Ad es. http://www.mercanteweb.it/Templates/
         /// </summary>
         public static Uri GlobalTemplatesPath
         {
            get
            {
               string path = string.Concat(GetSiteRoot(), "/Templates/");
               UriBuilder urb = new UriBuilder(path);
               return urb.Uri;
            }
         }


         /// <summary>
         /// Path relativo della cartella generale dei templates globali
         /// Ad es. ~/Templates/
         /// </summary>
         //public static string GlobalTemplatesRelativePath
         //{
         //   get
         //   {
         //      string path = GlobalTemplatesPath.ToString();
         //      return string.Concat("~", path.Substring(path.IndexOf(GetSiteRoot()) + GetSiteRoot().Length));
         //   }
         //}


         ///////// <summary>
         ///////// Path relativo della cartella generale dei templates globali
         ///////// Ad es. /Templates/{0}. Uso  String publicTemplate = String.Format(WebHelper.SpecialFolder.PublicTemplatePath, site.SiteId);
         ///////// </summary>
         //////public static string PublicTemplatePath
         //////{
         //////   get
         //////   {
         //////      return "/Templates/{0}";
         //////   }
         //////}

         /////// <summary>
         /////// Path relativo della cartella dei template custom (private...)
         /////// Ad es. /Templates/{0}. Uso  String customTemplate = String.Format(WebHelper.SpecialFolder.CustomTemplatePath, site.SiteId, template.TemplateId);
         /////// </summary>
         ////public static string CustomTemplatePath
         ////{
         ////   get
         ////   {
         ////      return "/Sites/{0}/Templates/{1}";
         ////   }
         ////}

         #endregion

         #region Site level Folders

         /// <summary>
         /// Uri della cartella "root" per un dato site
         /// Ad. es.: http://www.mercanteweb.it/Sites/0/
         /// </summary>
         /// <param name="siteId"></param>
         /// <returns></returns>
         public static Uri SiteRootPath(int siteId)
         {
            string sitePath = string.Concat(GlobalSitesPath, siteId.ToString(), "/");
            return new Uri(sitePath);
         }



         /// <summary>
         /// Path relativo della cartella root per un dato Site
         /// Ad. es.: ~/Sites/0/
         /// </summary>
         public static string SiteRootRelativePath(int siteId)
         {
            return string.Concat(GlobalSitesRelativePath, siteId.ToString(), "/");
         }


         /// <summary>
         /// Path relativo della cartella root per un WebExplorer
         /// Ad. es.: ~/Sites/0/WebExplorer
         /// </summary>
         public static string SiteWebExplorerRelativePath(int siteId)
         {
            string path = string.Concat(GlobalSitesRelativePath, siteId.ToString(), "/WebExplorer/");
            CheckIfPathExists(path, true);
            return path;
         }



         /////////// <summary>
         /////////// Uri della cartella delle immagini specifica per un dato Site
         /////////// Ad. es.: http://www.mercanteweb.it/Sites/0/images/
         /////////// </summary>
         ////////public static Uri SiteImagesPath(int siteId)
         ////////{
         ////////   string path = string.Concat(GlobalSitesPath, siteId.ToString(), "/images/");
         ////////   CheckIfPathExists(path, true);
         ////////   return new Uri(path);
         ////////}


         /////////// <summary>
         /////////// Path relativo della cartella delle immagini specifica per un dato Site
         /////////// Ad. es.: ~/Sites/0/images/
         /////////// </summary>
         ////////public static string SiteImagesRelativePath(int siteId)
         ////////{
         ////////   string path = string.Concat(GlobalSitesRelativePath, siteId.ToString(), "/images/");
         ////////   CheckIfPathExists(path, true);
         ////////   return path;
         ////////}


         ///// <summary>
         ///// Uri della cartella dei template specifici per un dato Site
         ///// Ad. es.: http://www.mercanteweb.it/Sites/0/templates/
         ///// </summary>
         //public static Uri SiteTemplatesPath(int siteId)
         //{
         //   string path = string.Concat(GlobalSitesPath, siteId.ToString(), "/templates/");
         //   CheckIfPathExists(path, true);
         //   return new Uri(path);
         //}



         ///// <summary>
         ///// Path relativo della cartella dei template specifici per un dato Site
         ///// Ad. es.: ~/Sites/0/templates/
         ///// </summary>
         ///// <param name="siteId"></param>
         ///// <returns></returns>
         //public static string SiteTemplatesRelativePath(int siteId)
         //{
         //   string path = string.Concat(GlobalSitesRelativePath, siteId.ToString(), "/templates/");
         //   CheckIfPathExists(path, true);
         //   return path;
         //}



         private static void CheckIfPathExists(string path, bool createIfNotExists)
         {
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            if (!dir.Exists && createIfNotExists)
            {
               dir.Create();
            }
         }

         #endregion

      }

      #endregion

      #region HttpResponse Methods

      public static class HttpResponse
      {
         #region Protected Respond Methods

         /// <summary>
         /// Helper method used to Respond to the request that an error occurred in 
         /// processing the request.
         /// Equivalent to HTTP status 500
         /// </summary>
         /// <param name="context">Context.</param>
         public static void RespondInternalError(HttpContext context)
         {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Flush();
            context.Response.End();
         }



         /// <summary>
         /// Helper method used to Respond to the request that the request in attempting 
         /// to access a resource that the user does not have access to.
         /// Equivalent to HTTP status 403
         /// </summary>
         /// <param name="context">Context.</param>
         public static void RespondForbidden(HttpContext context)
         {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.Flush();
            context.Response.End();
         }



         /// <summary>
         /// Helper method used to Respond to the request that the file was not found.
         /// Equivalent to HTTP status 404
         /// </summary>
         /// <param name="context">Context.</param>
         public static void RespondFileNotFound(HttpContext context)
         {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Flush();
            context.Response.End();
         }

         /// <summary>
         /// Helper method used to Respond to the request that the file was not found.
         /// Equivalent to HTTP status 503 Service Unavailable
         /// </summary>
         /// <param name="context">Context.</param>
         /// <param name="flushResponse">Se a true chiude il buffer di risposta http</param>
         public static void RespondSiteClosed(HttpContext context, bool flushResponse)
         {
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
            context.Response.StatusDescription = "This site is temporarly closed for maintenance. Il sito è temporaneamente chiuso per manutenzione.";

            if (flushResponse)
            {
               context.Response.Flush();
               context.Response.End();
            }
         }

         #endregion

      }

      #endregion
   }
}