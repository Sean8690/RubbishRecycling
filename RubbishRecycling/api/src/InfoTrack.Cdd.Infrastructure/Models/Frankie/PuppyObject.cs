namespace InfoTrack.Cdd.Infrastructure.Models.Frankie
{
    /// <summary>
    /// Frankie's ping response.
    ///
    /// Frankie: All valid customers get a puppy. Otherwise, no puppy for you!
    /// </summary>
    public class PuppyObject
    {
        /// <summary>
        /// Ping status
        /// </summary>
        public bool Puppy { get; set; }
        
        /// <summary>
        /// Server version indication
        /// </summary>
        public string Commit { get; set; }
    }
}
