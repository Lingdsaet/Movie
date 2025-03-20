using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Movie.Models;
using Movie.Repository;
using Movie.ResponseDTO;


namespace Movie.ControllerWeb
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/User/SignUp
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("Dữ liệu người dùng không hợp lệ");

            var existingUser = await _userRepository.GetUserByUserNameAsync(userDTO.UserName);
            if (existingUser != null)
                return Conflict("Tên người dùng đã tồn tại");

            var user = new User
            {
                UserName = userDTO.UserName,
                Password = userDTO.Password, 
                Email = userDTO.Email
            };

            await _userRepository.CreateUserAsync(user);

            return Ok("Người dùng được tạo thành công");
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("Dữ liệu người dùng không hợp lệ");

            var user = await _userRepository.GetUserByUserNameAsync(userDTO.UserName);

            if (user == null || user.Password != userDTO.Password)
                return Unauthorized("Tên người dùng hoặc mật khẩu không hợp lệ");

            return Ok("Đăng nhập thành công");
        }
    }

}