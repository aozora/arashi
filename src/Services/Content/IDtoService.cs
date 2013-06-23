using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;

namespace Arashi.Services.Content
{
   public interface IDtoService
   {
      TemplateContentDTO GetTemplateContentDTO(Site site);


   }
}
