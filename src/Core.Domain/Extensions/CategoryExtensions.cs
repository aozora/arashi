using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;

namespace Arashi.Core.Domain.Extensions
{
   public static class CategoryExtensions
   {


      public static void SetParentCategory(this Category category, Category newParentCategory)
      {
         if (category.Id != -1 && newParentCategory == category.ParentCategory)
         {
            return; // don't do anything when the parent stays the same.
         }

         if (category.ParentCategory != null)
         {
            IList<Category> categoryList = category.ParentCategory.ChildCategories;

            categoryList.Remove(category);
            
            // Re-organize sibling positions.
            for (int i = 0; i < categoryList.Count; i++)
            {
               categoryList[i].SetPosition(i);
            }
         }
         else if (category.Site.RootCategories.Contains(category))
         {
            category.Site.RootCategories.Remove(category);
            // Re-organize sibling positions.
            for (int i = 0; i < category.Site.RootCategories.Count; i++)
            {
               category.Site.RootCategories[i].SetPosition(i);
            }
         }

         category.ParentCategory = newParentCategory;
         if (newParentCategory != null)
         {
            category.SetPosition(newParentCategory.ChildCategories.Count);
            newParentCategory.ChildCategories.Add(category);
         }
         else
         {
            category.SetPosition(category.Site.RootCategories.Count);
            category.Site.RootCategories.Add(category);
         }
      }



      public static void SetPosition(this Category category, int position)
      {
         category.Position = position;
         
         // update the path
         category.SyncPath();
      }



      /// <summary>
      /// Synchronize the path with the position and also of all child categories.
      /// </summary>
      private static void SyncPath(this Category category)
      {
         category.Path = "." + category.Position.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
         if (category.ParentCategory != null)
         {
            category.Path = category.ParentCategory.Path + category.Path;
         }
         // Recurse into child categories
         foreach (Category childCategory in category.ChildCategories)
         {
            childCategory.SyncPath();
         }
      }

   }
}
