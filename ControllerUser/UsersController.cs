using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.ResponseDTO;


namespace Movie.ControllerWeb
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserUserRepository _userRepository;

        public UserController(IUserUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/User/SignUp
        [HttpPost("SignUp-User")]
        public async Task<IActionResult> CreateUser([FromForm] string username, [FromForm] string email, [FromForm] string password)
        {
            try
            {
                var result = await _userRepository.RegisterAsync(username, email, password);
                if (result == null)
                {
                    return BadRequest(new { message = "Email hoặc Username thằng khác đã dùng!" });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo người dùng." });
            }
        }


        // POST: api/User/login
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            var result = await _userRepository.LoginAsync(email, password);

            if (result == null)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu rồi cưng!" });
            }

            return Ok(new
            {
                message = "Đăng nhập thành công!",
                result.UserId,
                result.UserName,
                result.Email,
                result.Password,
                result.CreatedDate
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }
    }

}