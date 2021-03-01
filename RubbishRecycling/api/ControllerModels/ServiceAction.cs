using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.ControllerModels
{
    public class ServiceActionResult
    {
        public bool CanProceed { get; set; }
        public string Message { get; set; }
        public int StatusCodes { get; set; }
    }
}
