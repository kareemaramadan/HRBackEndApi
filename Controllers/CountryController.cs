using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Country;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.LookUps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Linq.Expressions;


namespace HRBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(IBaseService<Country> countryService, IMapper mapper) : ControllerBase
    {
        private readonly IBaseService<Country> _countryService = countryService;
        private readonly IMapper _mapper = mapper;


        [HttpPost]
        [Route("CreateCountry")]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto CreateCountry)
        {
            if (CreateCountry == null)
            {
                return BadRequest("Country object is null");
            }
            Country mappedcountry = _mapper.Map<Country>(CreateCountry);
            var createdCountry = await _countryService.CreateAsync(mappedcountry);

            return CreatedAtAction(nameof(CreateCountry), _mapper.Map<GetCountryDto>(createdCountry));
        }


        [HttpPost]
        [Route("AddCountry")]
        public async Task<ActionResult> CUDUsingStoredProcedure([FromBody] CreateCountryDto? CreateCountry)
        {
            if (CreateCountry == null)
            {
                return BadRequest("You Cannot add an Empty values");
            }
            if (string.IsNullOrWhiteSpace(CreateCountry.CountryName_en) || string.IsNullOrWhiteSpace(CreateCountry.CountryName_ar))
            {
                return BadRequest("CountryName_en and CountryName_ar must be provided.");
            }

            Country mappedCountry = _mapper.Map<Country>(CreateCountry);

            var parametres = new Dictionary<string, object>
            {
                { "CountryName_en", mappedCountry.CountryName_en! },
                { "CountryName_ar", mappedCountry.CountryName_ar! }
            };

            Expression<Func<Country, bool>> expression = cou => cou.CountryName_en == mappedCountry.CountryName_en || cou.CountryName_ar == mappedCountry.CountryName_ar;
            

            var createdCountry = await _countryService.CUDUsingStoredProcedureAsync("SP_Insert_Country", parametres,expression, HttpRequestType.Post);
            
            return (createdCountry > 0) ? Ok(new { message = "Your Country has been created successfully." }) : BadRequest(new { message = "This item is already exists" });


        }

        [HttpGet]
        [Route("GetAllCountriesByORM")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllAsync();
            var countryDtos = _mapper.Map<IEnumerable<GetCountryDto>>(countries);
            return Ok(countryDtos); 
        }
        //
        [HttpGet]
        [Route("SP_GetAllCountries")]
        public async Task<IActionResult> GetAll()
        {
            Dictionary<string,object> parameters = new Dictionary<string, object> { };
            var countries = await _countryService.GetUsingStoredProcedureAsync("SP_GetAll_Countries",parameters);

            var countryDtos = _mapper.Map<IEnumerable<GetCountryDto>>(countries);
            return Ok(countryDtos.Where(static c => c.CountryName_en!.StartsWith('F')).ToList());
        }




    }
}
