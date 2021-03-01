using System.ComponentModel.DataAnnotations;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    public enum IbResponseCode
    {
        [Display(Name="Success")]
        Success = 100,
        [Display(Name = "Server error")]
        ServerError = 500,
        [Display(Name = "No results")]
        NoResults = 102,
        [Display(Name = "Registry not available")]
        RegistryNotAvailable = 104,
        [Display(Name = "Company not found")]
        CompanyNotFound = 105,
        [Display(Name = "Invalid registry authority")]
        InvalidRegistryAuthority = 106,
        [Display(Name = "Parameter not supported")]
        ParameterNotSupported = 107,
        [Display(Name = "Too many matches")]
        TooManyMatches = 109,
        [Display(Name = "Company number not valid format")]
        CompanyNumberNotValidFormat = 110
    }
}
