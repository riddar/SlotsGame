using Microsoft.AspNetCore.Mvc;
using SlotGame.API.Services;
using SlotGame.Types.Contracts.Requests;
using SlotGame.Types.Contracts.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace SlotGame.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentitiesController: Controller
    {
        private readonly IIdentityService service;
        public IdentitiesController(IIdentityService _service)
        {
            service = _service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage))
                });
                

            var authResponse = await service.RegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse { Errors = authResponse.Errors });

            return Ok(new AuthSuccessResponse { Token=authResponse.Token });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await service.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
                return BadRequest(new AuthFailedResponse { Errors = authResponse.Errors });

            return Ok(new AuthSuccessResponse { Token = authResponse.Token });
        }

    }
}
