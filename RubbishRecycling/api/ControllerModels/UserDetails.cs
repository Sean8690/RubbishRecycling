using RubbishRecyclingAU.Models;
using System.ComponentModel.DataAnnotations;

namespace RubbishRecyclingAU.ControllerModels
{
    public class UserDetails
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string AvatarUrl { get; set; }

        public AddressModel Address { get; set; }
    }
}
