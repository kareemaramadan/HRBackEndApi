using AutoMapper;
using HR.Application.Dtos.LookUpDtos.Country;
using HR.Application.Interfaces;
using HR.Domain.Models.LookUps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Configuration;


namespace HRBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(IBaseService<Country> countryService, IMapper mapper) : ControllerBase
    {
        private readonly IBaseService<Country> _countryService = countryService;
        private readonly IMapper _mapper = mapper;


        [HttpPost]
        [Route("AddCountryByORM")]
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
        public async Task<ActionResult<GetCountryDto>> CUDUsingStoredProcedure([FromBody] CreateCountryDto? CreateCountry)
        {
            if (CreateCountry == null)
            {
                return BadRequest("Invalid stored procedure data.");
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

            var  createdCountry =  await _countryService.CUDUsingStoredProcedureAsync("SP_Insert_Country", parametres);
            return Ok(new { message = $"{createdCountry}Your Country has been created successfully." });
        }



        [HttpGet]
        [Route("GetAllCountriesByORM")]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllAsync();
            var countryDtos = _mapper.Map<IEnumerable<GetCountryDto>>(countries);
            return Ok(countryDtos); 
        }




    }
}
