using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.User
{
    public  class UserCreateDTO
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string Password { get; set; }
    }
}
