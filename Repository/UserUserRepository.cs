﻿using Movie.Models;
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

            var user = await _context.Users
                .Where(u => u.Email == email && u.Status == 1 && u.Role == false)
                .FirstOrDefaultAsync();


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
        

    }
}