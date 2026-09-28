using HR.Application.Interfaces;
using HR.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;


namespace HR.Application.Services
{
 public class LocalizationService ( IStringLocalizer<Translation> localizer ) : ILocalizationService
 {
  public string Get ( string key )
  {
   return localizer.GetString ( key );
  }

  public string GetLanguage ( )
  {
   return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
  }
 }

}
