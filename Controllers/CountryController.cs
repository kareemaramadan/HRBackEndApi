using Microsoft.AspNetCore.Mvc;
using HR.Application.Interfaces;
using HR.Application.Dtos.LookUpDtos.Country;
using HR.Domain.Models.LookUps;
using AutoMapper;


namespace HRBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController(IBaseService<Country> countryService, IMapper mapper) : ControllerBase
    {
        private readonly IBaseService<Country> _countryService = countryService;
        private readonly IMapper _mapper = mapper;

        [HttpPost]

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
    }
}
