using Business_Logic_Layer.UserServices;
using Data.Models.Models.Request.Login;
using Data.Models.Models.Response.LoginResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyShow.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IUserService _userRepository;

        public Authentication(IUserService userService)
        {
            _userRepository = userService;
        }
        /// <summary>
        /// login here who are register 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var result = _userRepository.LoginUser(model);
                if (result)
                {
                    // Normally you would return a JWT token here
                    return Ok("Login successful");
                }
                return Unauthorized("Invalid username or password.");
            }
            catch 
            {
                return BadRequest("Invalid username or password.");
            }
           

           
        }
    }
}
