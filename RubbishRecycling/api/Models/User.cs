using System.Collections.Generic;

namespace RubbishRecyclingAU.Models
{
    ///<Summary>
    /// User model
    ///</Summary>
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}