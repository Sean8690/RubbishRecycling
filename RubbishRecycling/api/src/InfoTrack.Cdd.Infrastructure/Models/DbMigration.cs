using System;
using MongoDB.Bson.Serialization.Attributes;

namespace InfoTrack.Cdd.Infrastructure.Models
{
    /// <summary>
    /// A database snapshot
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DbMigration 
    {
        [BsonId]
        public MongoDB.Bson.BsonObjectId Id { get; set; }
        public DateTime Applied { get; set; }
        public string Name { get; set; }
        public DbMigrationType Type { get; set; }
    }

    public enum DbMigrationType
    {
        Unknown,
        /// <summary>
        /// The entire snapshot was applied (db was replaced)
        /// </summary>
        Snapshot,
        /// <summary>
        /// A migration update was applied (db was updated)
        /// </summary>
        Migration
    }
}
