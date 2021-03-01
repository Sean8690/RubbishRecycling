using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class CompanyProfileDtoExtensions
    {
        public static bool HasDirectorsOrShareholders(this CompanyProfileDto data)
            => data?.DirectorAndShareDetails != null ||
               (data?.Officers != null && data.Officers.Count > 0);

        public static bool HasLegalForm(this CompanyProfileDto data)
            => data?.LegalFormDetails != null || data?.LegalFormDeclaration != null /*|| data?.LegalForm != null*/;

        public static bool HasCapital(this CompanyProfileDto data)
            => data?.Capital != null && data?.Capital.Count > 0;

        public static bool HasActivityData(this CompanyProfileDto data)
            => (data?.ActivityDeclarations != null && data.ActivityDeclarations.HasAnyData())
                || (data?.Activities != null && data.Activities.HasAnyData());

        public static bool HasAddressOrContactData(this CompanyProfileDto data)
            => (data?.Addresses != null && data.Addresses.Any(a => a.HasData()))
               || !string.IsNullOrWhiteSpace(data?.Headquarters)
               || !string.IsNullOrWhiteSpace(data?.MailingAddress)
               || !string.IsNullOrWhiteSpace(data?.Email)
               || !string.IsNullOrWhiteSpace(data?.FaxNumber)
               || !string.IsNullOrWhiteSpace(data?.TelephoneNumber)
               || !string.IsNullOrWhiteSpace(data?.WebsiteUrl);

        public static string GetCompanyNumber(this CompanyProfileDto data)
            => !string.IsNullOrWhiteSpace(data?.CompanyNumber) ? data.CompanyNumber 
                : !string.IsNullOrWhiteSpace(data?.RegistrationNumber) ? data.RegistrationNumber 
                : null;

        public static bool HasAnyPhone(this CompanyProfileDto data)
            => data != null && (data.Addresses.HasAnyPhone() || !string.IsNullOrWhiteSpace(data.TelephoneNumber));

        public static bool HasAnyFax(this CompanyProfileDto data)
            => data != null && (data.Addresses.HasAnyFax() || !string.IsNullOrWhiteSpace(data.FaxNumber));

        public static bool HasAnyEmail(this CompanyProfileDto data)
            => data != null && (data.Addresses.HasAnyEmail() || !string.IsNullOrWhiteSpace(data.Email));

        public static bool HasAnyWebsite(this CompanyProfileDto data)
            => data != null && (data.Addresses.HasAnyWebsite() || !string.IsNullOrWhiteSpace(data.WebsiteUrl));
    }
}
