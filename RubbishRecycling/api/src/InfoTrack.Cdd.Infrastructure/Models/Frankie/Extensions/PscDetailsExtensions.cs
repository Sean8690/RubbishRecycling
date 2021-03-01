using System.Text;

namespace InfoTrack.Cdd.Infrastructure.Models.Frankie.Extensions
{
    public static class PscDetailsExtensions
    {
        public static string DateOfBirth(this PscDetails psc)
        {
            var sb = new StringBuilder();
            if (psc?.DobYear != null)
            {
                sb.Append(psc.DobYear);
            }
            if(psc?.DobMonth != null)
            {
                sb.AppendFormat("{0}-", psc.DobMonth);
            }
            if (psc?.DobDay != null)
            {
                sb.AppendFormat("{0}-", psc.DobDay);
            }
            return sb.ToString();
        }
    }
}
