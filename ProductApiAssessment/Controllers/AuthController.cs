using Application.DTOs;
using Application.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductApiAssessment.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequestDto dto)
        {
            await _authService.RevokeTokenAsync(dto.RefreshToken);
            return NoContent();
        }
    }
}
