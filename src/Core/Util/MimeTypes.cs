using System;
using System.Collections.Generic;
using System.Linq;

namespace Arashi.Core.Util
{
   public class MimeTypes
   {
      private static Dictionary<String, String> mimeTypesHash = new Dictionary<string, string>();


      static MimeTypes()
      {
         CreateHash();
      }


      private static void CreateHash()
      {
         mimeTypesHash.Add("fif", "application/fractals");
         mimeTypesHash.Add("hta", "application/hta");
         mimeTypesHash.Add("hqx", "application/mac-binhex40");
         mimeTypesHash.Add("vsi", "application/ms-vsi");
         mimeTypesHash.Add("accdb", "application/msaccess");
         mimeTypesHash.Add("one", "application/msonenote");
         mimeTypesHash.Add("doc", "application/msword");
         mimeTypesHash.Add("p10", "application/pkcs10");
         mimeTypesHash.Add("p7m", "application/pkcs7-mime");
         mimeTypesHash.Add("p7s", "application/pkcs7-signature");
         mimeTypesHash.Add("cer", "application/pkix-cert");
         mimeTypesHash.Add("crl", "application/pkix-crl");
         mimeTypesHash.Add("ps", "application/postscript");
         mimeTypesHash.Add("smil", "application/smil");
         mimeTypesHash.Add("xls", "application/vnd.ms-excel");
         mimeTypesHash.Add("xlam", "application/vnd.ms-excel.addin.macroEnabled.12");
         mimeTypesHash.Add("xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12");
         mimeTypesHash.Add("xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12");
         mimeTypesHash.Add("xltm", "application/vnd.ms-excel.template.macroEnabled.12");
         mimeTypesHash.Add("mpf", "application/vnd.ms-mediapackage");
         mimeTypesHash.Add("thmx", "application/vnd.ms-officetheme");
         mimeTypesHash.Add("rels", "application/vnd.ms-package.relationships+xml");
         mimeTypesHash.Add("sst", "application/vnd.ms-pki.certstore");
         mimeTypesHash.Add("pko", "application/vnd.ms-pki.pko");
         mimeTypesHash.Add("cat", "application/vnd.ms-pki.seccat");
         mimeTypesHash.Add("stl", "application/vnd.ms-pki.stl");
         mimeTypesHash.Add("ppt", "application/vnd.ms-powerpoint");
         mimeTypesHash.Add("vdx", "application/vnd.ms-visio.viewer");
         mimeTypesHash.Add("wpl", "application/vnd.ms-wpl");
         mimeTypesHash.Add("xps", "application/vnd.ms-xpsdocument");
         mimeTypesHash.Add("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
         mimeTypesHash.Add("sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
         mimeTypesHash.Add("ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
         mimeTypesHash.Add("potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
         mimeTypesHash.Add("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
         mimeTypesHash.Add("xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
         mimeTypesHash.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
         mimeTypesHash.Add("dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
         mimeTypesHash.Add("rm", "application/vnd.rn-realmedia");
         mimeTypesHash.Add("rsml", "application/vnd.rn-rsml");
         mimeTypesHash.Add("z", "application/x-compress");
         mimeTypesHash.Add("tgz", "application/x-compressed");
         mimeTypesHash.Add("gz", "application/x-gzip");
         mimeTypesHash.Add("ins", "application/x-internet-signup");
         mimeTypesHash.Add("iii", "application/x-iphone");
         mimeTypesHash.Add("jnlp", "application/x-java-jnlp-file");
         mimeTypesHash.Add("jtx", "application/x-jtx+xps");
         mimeTypesHash.Add("latex", "application/x-latex");
         mimeTypesHash.Add("nix", "application/x-mix-transfer");
         mimeTypesHash.Add("asx", "application/x-mplayer2");
         mimeTypesHash.Add("application", "application/x-ms-application");
         mimeTypesHash.Add("wmd", "application/x-ms-wmd");
         mimeTypesHash.Add("wmz", "application/x-ms-wmz");
         mimeTypesHash.Add("xbap", "application/x-ms-xbap");
         mimeTypesHash.Add("p12", "application/x-pkcs12");
         mimeTypesHash.Add("p7b", "application/x-pkcs7-certificates");
         mimeTypesHash.Add("p7r", "application/x-pkcs7-certreqresp");
         mimeTypesHash.Add("skype", "application/x-skype");
         mimeTypesHash.Add("sit", "application/x-stuffit");
         mimeTypesHash.Add("tar", "application/x-tar");
         mimeTypesHash.Add("man", "application/x-troff-man");
         mimeTypesHash.Add("xpi", "application/x-xpinstall;app=firefox");
         mimeTypesHash.Add("zip", "application/zip");
         mimeTypesHash.Add("xaml", "application/xaml+xml");
         mimeTypesHash.Add("aiff", "audio/aiff");
         mimeTypesHash.Add("au", "audio/basic");
         mimeTypesHash.Add("mid", "audio/midi");
         mimeTypesHash.Add("mp3", "audio/mp3");
         mimeTypesHash.Add("m3u", "audio/mpegurl");
         mimeTypesHash.Add("ra", "audio/vnd.rn-realaudio");
         mimeTypesHash.Add("rp", "audio/vnd.rn-realpix");
         mimeTypesHash.Add("wav", "audio/wav");
         mimeTypesHash.Add("bmp", "image/bmp");
         mimeTypesHash.Add("svg", "image/svg+xml");
         mimeTypesHash.Add("gif", "image/gif");
         mimeTypesHash.Add("jpg", "image/jpeg");
         mimeTypesHash.Add("png", "image/png");
         mimeTypesHash.Add("tiff", "image/tiff");
         mimeTypesHash.Add("mdi", "image/vnd.ms-modi");
         mimeTypesHash.Add("ico", "image/x-icon");
         mimeTypesHash.Add("pntg", "image/x-macpaint");
         mimeTypesHash.Add("qtif", "image/x-quicktime");
         mimeTypesHash.Add("ics", "text/calendar");
         mimeTypesHash.Add("css", "text/css");
         mimeTypesHash.Add("323", "text/h323");
         mimeTypesHash.Add("htm", "text/html");
         mimeTypesHash.Add("uls", "text/iuls");
         mimeTypesHash.Add("txt", "text/plain");
         mimeTypesHash.Add("master", "text/plain");
         mimeTypesHash.Add("wsc", "text/scriptlet");
         mimeTypesHash.Add("rt", "text/vnd.rn-realtext");
         mimeTypesHash.Add("htt", "text/webviewhtml");
         mimeTypesHash.Add("htc", "text/x-component");
         mimeTypesHash.Add("iqy", "text/x-ms-iqy");
         mimeTypesHash.Add("odc", "text/x-ms-odc");
         mimeTypesHash.Add("rqy", "text/x-ms-rqy");
         mimeTypesHash.Add("vcf", "text/x-vcard");
         mimeTypesHash.Add("xml", "text/xml");
         mimeTypesHash.Add("avi", "video/avi");
         mimeTypesHash.Add("mpeg", "video/mpeg");
         mimeTypesHash.Add("mpg", "video/mpeg");
         mimeTypesHash.Add("mov", "video/quicktime");
         mimeTypesHash.Add("rv", "video/vnd.rn-realvideo");
         mimeTypesHash.Add("wm", "video/x-ms-wm");
         mimeTypesHash.Add("wmv", "video/x-ms-wmv");
         mimeTypesHash.Add("wmx", "video/x-ms-wmx");
         mimeTypesHash.Add("wvx", "video/x-ms-wvx");
         mimeTypesHash.Add("pdf", "application/pdf");
         mimeTypesHash.Add("swf", "application/x-shockwave-flash");
      }


      /// <summary>
      /// Returns the MIME-Type name (i.e. "video/mpg") given a file extension  (without the dot).
      /// If there are not matches for the given extensions an empty string is returned.
      /// </summary>
      /// <param name="extension"></param>
      /// <returns></returns>
      public static string GetMimeTypeName(string extension)
      {
         if (mimeTypesHash.ContainsKey(extension))
            return mimeTypesHash[extension];
         else
            return string.Empty;
      }



      /// <summary>
      /// return true if the given extension if for a file image type 
      /// </summary>
      /// <param name="extension"></param>
      /// <returns></returns>
      public static bool IsImage(string extension)
      {
         if (mimeTypesHash.ContainsKey(extension) && (extension == "bmp") || extension == "gif" || extension == "jpg" || extension == "png" || extension == "tiff" || extension == "ico")
            return true;

         return false;
      }



      /// <summary>
      /// return true if the given extension if for a file movie type 
      /// </summary>
      /// <param name="extension"></param>
      /// <returns></returns>
      public static bool IsMovie(string extension)
      {
         if (mimeTypesHash.ContainsKey(extension) && (extension == "avi") || extension == "mpeg" || extension == "mpg" || extension == "mov" || extension == "wmv" || extension == "swf")
            return true;

         return false;
      }



      /// <summary>
      /// return true if the given extension if for a file audio type 
      /// </summary>
      /// <param name="extension"></param>
      /// <returns></returns>
      public static bool IsAudio(string extension)
      {
         if (mimeTypesHash.ContainsKey(extension) && (extension == "mid") || extension == "mp3" || extension == "ra" || extension == "wav")
            return true;

         return false;
      }


   }
}