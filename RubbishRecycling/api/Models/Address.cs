using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetOrUnitNumber { get; set; }
        public string StreetName { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public int? UserId { get; set; }
        public int? ProductId { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
