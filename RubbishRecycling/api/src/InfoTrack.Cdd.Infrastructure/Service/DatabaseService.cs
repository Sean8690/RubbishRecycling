using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrack.Cdd.Infrastructure.Config;
using InfoTrack.Cdd.Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InfoTrack.Cdd.Infrastructure.Service
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Country>> GetCountries();

        Task<Country> GetCountry(string kyckrCountryCode);

        /// <summary>
        /// Overwrite countries seed data with a new collection
        /// </summary>
        Task ApplyCountriesSnapshot(IEnumerable<Country> countries, string snapshotName);

        Task<DbMigration> GetLatestMigration(DbMigrationType type);
    }

    public class DatabaseService : IDatabaseService
    {
        private readonly IMongoDatabase _database;

        public DatabaseService(Mongo mongo, IMongoClient mongoClient)
        {
            if (mongo == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }

            if (mongoClient == null)
            {
                throw new ArgumentNullException(nameof(mongoClient));
            }
            
            _database = mongoClient.GetDatabase(mongo.DatabaseName);
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var db = _database.GetCollection<Country>("Countries");
            return await db.Find(x => !string.IsNullOrEmpty(x.KyckrCountryCode)).ToListAsync();
        }

        public async Task<Country> GetCountry(string kyckrCountryCode)
        {
            var db = _database.GetCollection<Country>("Countries");
            return await db.Find(x => x.KyckrCountryCode == kyckrCountryCode.ToUpperInvariant()).SingleOrDefaultAsync();
        }

        /// <summary>
        /// This method can be used to update seed data
        /// </summary>
        protected internal async Task UpdateCountry(Country country)
        {
            await _database.GetCollection<Country>("Countries")
                .FindOneAndReplaceAsync(c => c.Id == country.Id, country);
        }

        /// <summary>
        /// Overwrite countries seed data with a new collection
        /// </summary>
        public async Task ApplyCountriesSnapshot(IEnumerable<Country> countries, string snapshotName)
        {
            var options = new ListCollectionNamesOptions { Filter = new BsonDocument("name", "Migrations") };
            var migrationsCollectionExists = await (await _database.ListCollectionNamesAsync(options)).AnyAsync();
            if (!migrationsCollectionExists)
            {
                await _database.CreateCollectionAsync("Migrations");
            }
            var snapshotCollection = _database.GetCollection<DbMigration>("Migrations");

            if (await snapshotCollection.Find(s => s.Name == snapshotName).SingleOrDefaultAsync() != null)
            {
                // This migration/snapshot has already been applied
                return;
            }

            await snapshotCollection.InsertOneAsync(new DbMigration
            {
                Name = snapshotName,
                Applied = DateTime.UtcNow,
                Type = DbMigrationType.Snapshot
            });

            var collection = _database.GetCollection<Country>("Countries");
            
            await collection.DeleteManyAsync(c => true);

            await collection.InsertManyAsync(countries);
        }

        public async Task<DbMigration> GetLatestMigration(DbMigrationType type)
        {
            var options = new FindOptions<DbMigration, DbMigration>
            {
                Limit = 1,
                Sort = Builders<DbMigration>.Sort.Descending(o => o.Applied)
            };

            var latest = (await _database.GetCollection<DbMigration>("Migrations")
                .FindAsync(f => f.Type == type, options)).FirstOrDefault();

            return latest;
        }
    }
}
