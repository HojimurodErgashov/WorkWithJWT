using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.User
{
    public class UserCredentials
    {
        [Required]
        [MinLength(8)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string Password { get; set; }
    }
}
