using Movie.Models;
using Movie.Repository;
using BCrypt.Net;
using System;
using Movie.ResponseDTO;
using Microsoft.EntityFrameworkCore;

namespace Movie.Repository
{
    public class UserUserRepository
    {
    }
}

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

        

        var user = new User
        {
            UserName = username,
            Email = email,
            Password = password,
            CreatedDate = DateTime.Now,
            Status = 1
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RequestUserDTO
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Password=user.Password,
            CreatedDate = user.CreatedDate
        };
    }


    public async Task<RequestUserDTO?> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || user.Password != password)
        {
            return null;
        }

        return new RequestUserDTO
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
        };
    }
    public async Task<IEnumerable<RequestUserDTO>> GetAllUsersAsync()
    {
        return await _context.Users
            .Where(u => u.Status == 1)
            .Select(u => new RequestUserDTO
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                CreatedDate = u.CreatedDate,
                Status = 1
            })
            .ToListAsync();
    }
}