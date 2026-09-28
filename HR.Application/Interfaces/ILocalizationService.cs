using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Application.Interfaces
{
 public interface ILocalizationService
 {
   string Get(string key);
  string GetLanguage ( );
 }
}
