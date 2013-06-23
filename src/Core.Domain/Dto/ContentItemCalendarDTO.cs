using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain.Dto
{
   public class ContentItemCalendarDTO
   {
      public virtual int Year {get; set;}
      public virtual int Month {get; set;}
      public virtual int Day {get; set;}
      public virtual long Count {get; set;}

   }
}
