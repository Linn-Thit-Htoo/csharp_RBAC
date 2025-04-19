using csharp_RBAC.Models;
using csharp_RBAC.Services;
using csharp_RBAC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace csharp_RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpGet("GetTokens")]
        public async Task<IActionResult> GetTokens()
        {
            Result<JwtResponseModel> result;
            try
            {
                JwtRequestModel jwtRequestModel = new()
                {
                    Email = "mglinnthithtoo@gmail.com",
                    Role = "Admin",
                    Scopes = new List<string> { "read", "write" }
                };
                var accessToken = _jwtService.GenerateAccessToken(jwtRequestModel);
                var refreshToken = _jwtService.GenerateRefreshToken(jwtRequestModel);

                var jwtResponseModel = new JwtResponseModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };

                result = Result<JwtResponseModel>.Success(jwtResponseModel);
            }
            catch (Exception ex)
            {
                result = Result<JwtResponseModel>.Fail(ex);
            }

            return Ok(result);
        }

        [HttpGet("AdminOnly")]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        public IActionResult AdminOnly()
        {
            return Ok("Admin Only!");
        }
    }
}
