using HR.Application.Dtos.AuthDtos;
using HR.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRBackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("RegisterUser")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto register)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            AuthDto result = await _authService.RegisterAsync(register);

            if(!result.IsAuthenticated)
             return BadRequest(result.Message);

            return Ok(result);
        }
    }
}