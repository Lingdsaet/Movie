﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Movie.Repository;
using Movie.Models;
using Movie.ResponseDTO;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AdminUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // POST: api/User/login
        [HttpPost("login")]

        public async Task<IActionResult> LoginUser(LoginDTO requestUser)

        {

            var token = await _userRepository.LoginUserAsync(requestUser.Email, requestUser.Password);

            if (token == null)

            {

                return Unauthorized(new { Messgae = "Tên tài khoản hoặc mật khẩu không tồn tại" });

            }

            return Ok(new { Message = "Đăng nhập thành công" });

        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string username, string newPassword)
        {
            var user = await _userRepository.GetUserByUserNameAsync(username);
            if (user == null)
            {
                return NotFound(new { Message = "Người dùng không tồn tại." });
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user.UserId, user.UserName, user.Email, hashedPassword);

            return Ok(new { Message = "Mật khẩu đã được cập nhật thành công." });
        }

        // GET: api/Users
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<RequestUserDTO>>> GetUsers(
            string? search = null,
            string sortBy = "id",
            string sortDirection = "asc")
        {
            var users = await _userRepository.GetAllUsersAsync(search, sortBy, sortDirection);
            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestUserDTO>> GetUser(int id)
        {
            // Lấy thông tin người dùng theo id và chỉ lấy những người có Status = 1
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null || user.Status != 1)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng hoặc người dùng đã bị xóa." });
            }

            return Ok(user);
        }
        // Fixed the invalid expression term ')' by removing the trailing comma in the method call.
       
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] UserDTO dto)
        {
            
            bool role = bool.Parse(dto.Role); 
            var result = await _userRepository.CreateUserAsync(dto.UserName, dto.Email, dto.Password, role);

            if (result == null)
                return BadRequest("Email hoặc Username đã tồn tại.");

            return Ok(result);
        }

        // PUT: api/Users/{id}
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(
        int id,
        string username,
        string email,
        string password)
        {
            try
            {
                var updatedUser = await _userRepository.UpdateUserAsync(id, username, email, password);

                if (updatedUser == null)
                {
                    return NotFound(new { Message = "Không tìm thấy người dùng." });
                }

                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(id);

            // Kiểm tra xem người dùng có tồn tại không
            if (user == null)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }

            // Kiểm tra nếu người dùng đã bị xóa rồi (Status = 0)
            if (user.Status == 0)
            {
                return BadRequest(new { Message = "Người dùng đã bị xóa trước đó." });
            }

            // Nếu người dùng còn sống (Status = 1), thay đổi trạng thái thành 0 (đã xóa)
            user.Status = 0;

            // Cập nhật lại trạng thái người dùng trong cơ sở dữ liệu
            bool updateSuccess = await _userRepository.UpdateUserStatusAsync(user.UserId, user.Status);
            if (updateSuccess)
            {
                return Ok(new { Message = "Người dùng đã được xóa thành công" });
            }
            else
            {
                return BadRequest(new { Message = "xóa người dùng không thành công." });
            }
        }

        // GET: api/Users/lichsu-xoa
        [HttpGet("lichsu-xoa")]
        public async Task<IActionResult> GetUsersDeleteHistory(
            string? search = null,  // Tìm theo tên người dùng (Username)
            string sortBy = "id",   // Trường sắp xếp (mặc định theo id)
            string sortDirection = "asc" // Hướng phân loại (asc/desc)

        )
        {
            // Gọi phương thức trong repository để lấy danh sách người dùng đã xóa với phân trang, tìm kiếm và sắp xếp
            var users = await _userRepository.GetUsersDeleteHistoryAsync(search, sortBy, sortDirection);

            if (users == null || users.Count == 0)
            {
                return NotFound(new { Message = "Không có người dùng nào đã bị xóa." });
            }

            return Ok(new
            {
                Users = users
            });
        }

        // DELETE: api/Users/Permanetly-Delete
        [HttpDelete("permanently-delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.PermanentlyDeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }

            return Ok(new { Message = "Xóa người dùng vĩnh viễn thành công." });
        }

    }
}