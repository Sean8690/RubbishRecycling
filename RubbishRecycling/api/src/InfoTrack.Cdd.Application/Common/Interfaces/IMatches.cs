namespace InfoTrack.Cdd.Application.Common.Interfaces
{
    /// <summary>
    /// Shallow "equality" comparer. May ignore irrelevant fields.
    /// </summary>
    public interface IMatches<T>
    {
        /// <summary>
        /// Shallow "equality" comparer. May ignore irrelevant fields.
        /// </summary>
        bool Matches(T other);
    }
}
