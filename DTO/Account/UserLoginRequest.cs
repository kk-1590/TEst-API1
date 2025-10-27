using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Account
{
    public class UserLoginRequest
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
