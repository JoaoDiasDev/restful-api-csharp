using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Data.VO;

namespace restful_api_joaodias.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness) { _loginBusiness = loginBusiness; }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Route("signin")]
        public IActionResult SignIn([FromBody] UserVO user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            var token = _loginBusiness.ValidateCredentials(user);

            if (token is null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVo)
        {
            if (tokenVo is null)
            {
                return BadRequest("Invalid client request");
            }

            var token = _loginBusiness.ValidateCredentials(tokenVo);

            if (token is null)
            {
                return BadRequest("Invalid client request");
            }
            return Ok(token);
        }


        [HttpGet]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var userName = User?.Identity?.Name;
            if (userName is null)
            {
                return BadRequest("Invalid client request");
            }
            var result = _loginBusiness.RevokeToken(userName);

            if (!result)
            {
                return BadRequest("Invalid client request");
            }
            return NoContent();
        }
    }
}
