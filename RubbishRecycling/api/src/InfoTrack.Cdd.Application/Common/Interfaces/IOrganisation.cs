namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Basic organisation info
    /// </summary>
    public interface IOrganisation
    {
        /// <summary>
        /// Organisation name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Organisation number, e.g. ABN or ACN
        /// </summary>
        string Number { get; set; }

        /// <summary>
        /// Country of registration. Kyckr-format country code (ISO2, but with documented exceptions for USA, Canada and UAE)
        /// </summary>
        string KyckrCountryCode { get; set; }
    }
}
