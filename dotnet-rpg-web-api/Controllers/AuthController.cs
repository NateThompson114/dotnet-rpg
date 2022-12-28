using dotnet_rpg_web_api.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg_web_api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody] UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User { Username = request.Username }, request.Password
            );

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> Login([FromBody] UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);

            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}
