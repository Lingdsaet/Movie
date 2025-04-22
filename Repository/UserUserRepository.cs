using Movie.Models;
using Movie.Repository;
using BCrypt.Net;
using System;
using Movie.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace Movie.Repository
{
    public class UserUserRepository : IUserUserRepository
    {
        private readonly movieDB _context;

        public UserUserRepository(movieDB context)
        {
            _context = context;

        }

        public async Task<RequestUserDTO?> RegisterAsync(string username, string email, string password)
        {
            // Kiểm tra nếu email đã tồn tại
            if (_context.Users.Any(u => u.Email == email))
            {
                return null; // Email đã tồn tại
            }

            // Kiểm tra nếu username đã tồn tại
            if (_context.Users.Any(u => u.UserName == username))
            {
                return null;
            }


            // Kiểm tra độ dài mật khẩu (phải từ 6 đến 100 ký tự)
            if (password.Length < 6 || password.Length > 100)
            {
                throw new ArgumentException("Mật khẩu phải có từ 6 đến 100 ký tự.");
            }

            // Băm mật khẩu trc khi lưu vào Db
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                UserName = username,
                Email = email,
                Password = hashedPassword,
                CreatedDate = DateTime.Now,
                Role = false,
                Status = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RequestUserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                CreatedDate = user.CreatedDate
            };
        }


        public async Task<RequestUserDTO?> LoginAsync(string email, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var query = _context.Users
                .Where(u => u.Status == 1)
                .Where(u => u.Role == false)  
                .AsQueryable();


            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return new RequestUserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password
            };
        }
        public async Task<RequestUserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            return new RequestUserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email
            };
        }
        public async Task<RequestUserDTO?> UpdateUserAsync(int id, string username, string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return null; // Trả về null nếu không tìm thấy User
            }

            // Kiểm tra nếu email đã tồn tại ở User khác
            if (_context.Users.Any(u => u.Email == email && u.UserId != id))
            {
                throw new ArgumentException("Email này đã được sử dụng bởi tài khoản khác.");
            }

            // Kiểm tra nếu username đã tồn tại ở User khác
            if (_context.Users.Any(u => u.UserName == username && u.UserId != id))
            {
                throw new ArgumentException("Tên người dùng đã tồn tại.");
            }

            // Kiểm tra độ dài mật khẩu
            if (password.Length < 6 || password.Length > 100)
            {
                throw new ArgumentException("Mật khẩu phải có từ 6 đến 100 ký tự.");
            }

            // Cập nhật thông tin User
            user.UserName = username;
            user.Email = email;
            user.Password = password;

            await _context.SaveChangesAsync();

            return new RequestUserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                Status = user.Status // Thêm Status vào DTO
            };
        }

    }
}