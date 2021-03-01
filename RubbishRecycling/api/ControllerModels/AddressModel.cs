using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels
{
    public class AddressModel
    {
        public string StreetOrUnitNumber { get; set; }
        public string StreetName { get; set; }
        [Required]
        public string Suburb { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
