using HR.Application.Interfaces;
using HR.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Globalization;


namespace HR.Application.Services
{
 public class LocalizationService ( IStringLocalizer<Translation> localizer, IHttpContextAccessor accessor) : ILocalizationService
 {
  public string Get ( string key )
  {
   return localizer.GetString ( key );
  }

  public string GetLanguage ( )
  {
            var currentLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            accessor.HttpContext.Response.Headers["Accept-Language"] = currentLanguage;
            return currentLanguage;
  }
 }

}
