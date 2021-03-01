using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool isExchanged { get; set; }

        public int AddressId { get; set; }
        public int UserId { get; set; }

        virtual public User User { get; set; }
        virtual public Address Address { get; set; }
    }
}
