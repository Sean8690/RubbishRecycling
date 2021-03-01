using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfoTrack.Cdd.Application.Dtos
{

    /// <summary>
    /// 
    /// </summary>
    public class AmlPersonLookupReportRequest
    {
        /// <summary>
        /// Parent OrderId (the Id of the lookup order)
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// Provider's unique entity identifiers (current provider is Frankie Financial)
        /// </summary>
        [Required]
        public IEnumerable<string> ProviderEntityCodes { get; set; }
    }
}
