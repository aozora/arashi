using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   public class TrackingInfo
   {
      #region Private Fields

      private int trackingInfoId;

      #endregion

      #region Public Properties

      public virtual int TrackingInfoId
      {
         get
         {
            return trackingInfoId;
         }
         set
         {
            trackingInfoId = value;
         }
      }

      public virtual string HostReferrer { get; set; }

      public virtual string UrlReferrer { get; set; }

      public virtual string TrackedUrl { get; set; }

      public virtual string HttpMethod { get; set; }

      public virtual User LoggedUser { get; set; }

      public virtual string AnonymousUserId { get; set; }

      public virtual string UserIp { get; set; }

      public virtual string UserLanguages { get; set; }

      public virtual string BrowserType { get; set; }

      public virtual string BrowserName { get; set; }

      public virtual string BrowserVersion { get; set; }

      public virtual string BrowserMajor { get; set; }

      public virtual string BrowserMinor { get; set; }

      public virtual string Platform { get; set; }

      public virtual DateTime TrackingDate { get; set; }

      #endregion

      #region Constructor

      public TrackingInfo()
      {
         trackingInfoId = -1;
      }

      #endregion

   }
}
