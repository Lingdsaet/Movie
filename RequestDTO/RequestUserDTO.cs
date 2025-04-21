using System;
using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class RequestUserDTO
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;
        public bool Role { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int Status { get; set; }  // 1: active, 0: deactivated

    }
}
