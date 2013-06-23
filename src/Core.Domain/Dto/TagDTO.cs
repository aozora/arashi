using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain.Dto
{
   [Serializable]
   public class TagDTO
   {
      public TagDTO()
      {
      }

      //public virtual Tag Tag {get; set;}

      public virtual int TagId {get; set;}
      public virtual string Name {get; set;}
      public virtual long Count {get; set;}

   }
}