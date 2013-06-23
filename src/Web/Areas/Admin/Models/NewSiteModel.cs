namespace Arashi.Web.Areas.Admin.Models
{
   using System.Web.Mvc;
   using System.ComponentModel.DataAnnotations;

   public class NewSiteModel
   {
      [Required(ErrorMessage = "The name of the new site must be specified.")]
      public string Name
      {
         get;
         set;
      }

      public string Description {get; set;}

      [Remote("CheckIfSiteHostExists", "Site", ErrorMessage = "Sorry, this host name is already in use. Please choose another one.")]
      [Required(ErrorMessage = "The default host name must be specified.")]
      public string DefaultHostName
      {
         get;
         set;
      }
   }
}
