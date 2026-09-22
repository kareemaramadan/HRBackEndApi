using AutoMapper;
using HR.Application.Dtos.AuthDtos;
using HR.Application.Dtos.LookUpDtos.Country;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.LookUps;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Linq.Expressions;


namespace HRBackEndApi.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class CountryController ( IBaseService<Country> countryService, IMapper mapper,ILogger<CountryController> logger ) : ControllerBase
    {
        //private readonly IBaseService<Country> _countryService = countryService;
        //private readonly IMapper _mapper = mapper;

        [HttpPost]
        [Route ( "AddNewCountry" )]
        public async Task<IActionResult> CreateCountry ( [FromBody] CreateCountryDto CreateCountry )
        {
            if ( CreateCountry == null )
            {
                return BadRequest ( "Fill missing Fields" );
            }
            Country mappedcountry = mapper.Map<Country> ( CreateCountry );
            var createdCountry = await countryService.CreateAsync ( mappedcountry, HttpRequestType.Post, c => c.CountryName_en == mappedcountry.CountryName_en || c.CountryName_ar == mappedcountry.CountryName_ar );
            logger.LogInformation ( $"created country `{createdCountry}` is created Successfully." );
            return CreatedAtAction ( nameof ( CreateCountry ), mapper.Map<GetCountryDto> ( createdCountry ) );

        }


        //[HttpPost]
        //[Route ( "AddNewCountryBySP" )]
        //public async Task<ActionResult> CreateCountryByStoredProcedure ( [FromBody] CreateCountryDto? CreateCountry, [FromQuery] string language )
        //{

        //    if ( CreateCountry == null )
        //    {
        //        return BadRequest ( "Fill missing Fields" );
        //    }
        //    if ( string.IsNullOrWhiteSpace ( CreateCountry.CountryName_en ) || string.IsNullOrWhiteSpace ( CreateCountry.CountryName_ar ) )
        //    {
        //        return BadRequest ( "Fill missing Fields" );
        //    }

        //    Country mappedCountry = _mapper.Map<Country> ( CreateCountry );

        //    var parametres = new Dictionary<string, object>
        //    {
        //        { "CountryName_en", mappedCountry.CountryName_en! },
        //        { "CountryName_ar", mappedCountry.CountryName_ar! }
        //    };

        //    Expression<Func<Country, bool>> expression = cou => cou.CountryName_en == mappedCountry.CountryName_en || cou.CountryName_ar == mappedCountry.CountryName_ar;


        //    var createdCountry = await _countryService.CUDUsingStoredProcedureAsync ( "SP_Insert_Country", parametres, expression, HttpRequestType.Post );

        //    return ( createdCountry > 0 ) ? Ok ( new { message = "Your Country has been created successfully." } ) : BadRequest ( new { message = "This item is already exists" } );


        //}

        [HttpGet]
        [Route ( "GetAllCountries" )]
        public async Task<ActionResult> GetAllCountries ( )
        {
            var (countries,IsSuccess) = await countryService.GetAllAsync ( );
            var countryDtos = mapper.Map<IEnumerable<GetCountryDto>> ( countries );
            return ( IsSuccess ) ? Ok ( countryDtos.ToList ( ) ) : NotFound ( "No items found." );
        }
        //
        //[HttpGet]
        //[Route ( "GetAllCountriesBySP" )]
        //public async Task<ActionResult> GetAll ( )
        //{
        //    Dictionary<string, object> parameters = new Dictionary<string, object> { };
        //    var countries = await _countryService.GetUsingStoredProcedureAsync ( "SP_GetAll_Countries", parameters );

        //    var countryDtos = _mapper.Map<IEnumerable<GetCountryDto>> ( countries );
        //    return (countryDtos.Count() > 0)?Ok(countryDtos.ToList()):NotFound("No items found.");
        //}

        [HttpPut]
        [Route("UpdateCountry/{countryId}")]
        public async Task<ActionResult> UpdateCountry ( [FromBody] UpdateCountryDto countryDto, [FromQuery] int countryId)
        {
            if(countryDto == null || countryId <=0 )
            {
                return BadRequest ("Fill the missing Fields");
            }
            Country country = mapper.Map<Country> ( countryDto );
            var (item, IsSuccess) = await countryService.UpdateAsync ( country, c => c.Id == countryId );

            return ( IsSuccess ) ? Ok ( "The item is updated Successfully" ) : BadRequest("update is failed.");
        }

        [HttpDelete]
        [Route ( "DeleteCountry/{countryId}" )]
        public async Task<ActionResult> DeleteCountry ( [FromQuery] string countryName ) 
        {
            
            if ( countryName == null )
            {
                return BadRequest ( "country Name is required" );
            }
            var (item, IsSuccess) = await countryService.FindAsync ( c => c.CountryName_ar == countryName || c.CountryName_en == countryName );
            if ( item == null || !item.Any ( ) )
            {
                return NotFound ( $"This item is not found." );
            }
            int rowsaffected = await countryService.DeleteAsync ( c => c.Id == item.First ( ).Id );
            return Ok ( $"The country {countryName} is deleted Successfully." );
        }



    }
}
