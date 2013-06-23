using System;

namespace Arashi.Core.Domain
{
   public enum WorkflowStatus
   {
      /// <summary>
      /// Work in progress
      /// </summary>
      Draft = 0,
      /// <summary>
      /// Currently being reviewed   
      /// </summary>
      Review = 1,
      /// <summary>
      /// Published 
      /// </summary>
      Published = 2
   }


   public enum CommentStatus
   {
      Unapproved = 0,
      Approved = 1,
      Spam = 2
   }



   public enum CommentType
   {
      Comment = 0,
      Pingback = 1,
      Trackback = 2
   }



   public enum SiteStatus
   {
      Online = 0,
      TemporaryDown = 1, // send HTTP 302 Status
      Offline = 2        // send HTTP 503 Status
   }




   public enum Editor
   {
      TinyMCE = 0,
      CodeMirror = 1
   }



   /// <summary>
   /// The target window of a link.
   /// </summary>
   public enum LinkTarget
   {
      /// <summary>
      /// Link opens in the same window.
      /// </summary>
      _self = 0,
      /// <summary>
      /// Link opens in new window.
      /// </summary>
      _blank = 1
   }




   public enum WidgetPlaceHolder
   {
      header = 0,
      content = 1,
      sidebar = 2, // for now I manage only 1 sidebar....
      footer = 3
   }




   /// <summary>
   /// The type of the current database.
   /// </summary>
   public enum DatabaseType
   {
      /// <summary>
      /// Microsoft SQL Server 2000 and up.
      /// </summary>
      MsSql,
      /// <summary>
      /// PostgreSQL 7.4 and up.
      /// </summary>
      PostgreSQL,
      /// <summary>
      /// MySQL 4.0 and up.
      /// </summary>
      MySQL
   }


}
