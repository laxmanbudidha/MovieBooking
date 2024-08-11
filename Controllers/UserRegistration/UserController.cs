using Business_Logic_Layer.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShow.Models.Request.Registration;

namespace MyShow.Controllers.UserRegistration
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// User Register here UserRegistrationRequest
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationRequest request)
        {

            if (ModelState.IsValid)
            {
                var success = _userService.RegisterUser(request);
                if (success)
                {
                    return Ok("User registered successfully.");
                }
                return BadRequest("User Email already exists.");
            }
            return BadRequest("Invalid input.");
        }
    }
}
