using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.User
{
    public class UserCredentials
    {
        [Required]
        [MinLength(8)]
        public string Login { get; set; }

        [Required]
        [StringLength(8)]
        public string Password { get; set; }
    }
}
