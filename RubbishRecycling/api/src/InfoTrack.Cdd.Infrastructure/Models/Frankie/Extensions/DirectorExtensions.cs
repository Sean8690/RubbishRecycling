using System;
using System.Collections.Generic;
using System.Linq;
using InfoTrack.Cdd.Infrastructure.Utils;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class DirectorExtensions
    {
        public static List<Directorship> UniqueDirectorships(this Director director, string excludeCompanyName, string excludeCompanyNumber)
            => director?.Directorships?.Where(
                   d => d != null &&
                        (!d.CompanyName.Equals(excludeCompanyName, System.StringComparison.OrdinalIgnoreCase) ||
                         !d.CompanyNumber.Equals(excludeCompanyNumber, StringComparison.OrdinalIgnoreCase))).ToList();

        public static List<Directorship> GetDirectorships(this Director director, string includeCompanyName, string includeCompanyNumber)
            => director?.Directorships?.Where(
                d => d?.CompanyName != null && d?.CompanyNumber != null &&
                     (d.CompanyName.Equals(includeCompanyName, System.StringComparison.OrdinalIgnoreCase) ||
                      d.CompanyNumber.Equals(includeCompanyNumber, StringComparison.OrdinalIgnoreCase))).ToList();

        public static string GetTitle(this Director director, string includeCompanyName, string includeCompanyNumber)
        {
            var directorships = director.GetDirectorships(includeCompanyName, includeCompanyNumber);
            if (directorships != null && directorships.Count > 0)
            {
                return string.Join("; ", directorships.Select(d => d.Function));
            }
            return director.Title;
        }

        public static string GetAppointedDate(this Director director, string includeCompanyName, string includeCompanyNumber)
        {
            var directorships = director.GetDirectorships(includeCompanyName, includeCompanyNumber);
            if (directorships != null && directorships.Count > 0)
            {
                return string.Join("; ", directorships.Select(d => DateHelper.TryFormatShortDate(d.AppointedDate)));
            }

            return null;
        }
    }
}
