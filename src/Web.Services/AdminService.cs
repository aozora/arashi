namespace Web.Services
{
   using Arashi.Core.NHibernate.Wcf;
   using System;

   /// <summary>
   /// Implementation of IAdminService
   /// </summary>
   [NHibernateContext]
   public class AdminService : IAdminService
   {


      public string GetData(int value)
      {
         return string.Format("You entered: {0}", value);
      }



      public CompositeType GetDataUsingDataContract(CompositeType composite)
      {
         if (composite == null)
         {
            throw new ArgumentNullException("composite");
         }
         if (composite.BoolValue)
         {
            composite.StringValue += "Suffix";
         }
         return composite;
      }


   }
}
