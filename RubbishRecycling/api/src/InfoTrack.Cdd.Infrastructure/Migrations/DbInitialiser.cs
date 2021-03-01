using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfoTrack.Cdd.Infrastructure.Models;
using InfoTrack.Cdd.Infrastructure.Service;
using InfoTrack.Cdd.Infrastructure.Utils;
using InfoTrack.Storage.Contracts.Api;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// This is probably a bit hacky and roll-your-own
// I very much welcome feedback if someone else knows a better way
// I didn't have time to do this properly :(

namespace InfoTrack.Cdd.Infrastructure.Migrations
{
    public interface IDbInitialiser
    {
        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        Task SeedCountries();

        Task UpdateFlags_20201221();
    }

    public class DbInitialiser : IDbInitialiser
    {
        private readonly IDatabaseService _databaseService;
        private readonly IStorageApi _storageApiClient;
        private readonly ILogger<DbInitialiser> _logger;

        public DbInitialiser(IDatabaseService databaseService, IStorageApi storageApiClient, ILogger<DbInitialiser> logger)
        {
            _databaseService = databaseService;
            _storageApiClient = storageApiClient;
            _logger = logger;
        }

        public async Task SeedCountries()
        {
            DbMigration latestAppliedSnapshot = await _databaseService.GetLatestMigration(DbMigrationType.Snapshot);

            var snapshotDirectory = new DirectoryInfo($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Migrations/Snapshots");
            
            var latestSnapshotFile = snapshotDirectory.GetFiles($"*_{In8nHelper.Region}.json")
                .OrderByDescending(f => f.Name).FirstOrDefault();

            if (latestSnapshotFile == null)
            {
                return;
            }

            var name = latestSnapshotFile.Name.Replace(latestSnapshotFile.Extension, "", StringComparison.OrdinalIgnoreCase);
            if (name != latestAppliedSnapshot?.Name)
            {
                var input = File.ReadAllText(latestSnapshotFile.FullName, Encoding.UTF8);
                var result = JsonConvert.DeserializeObject<List<Country>>(input);
                await _databaseService.ApplyCountriesSnapshot(result, name);
            }
        }

        public async Task UpdateFlags_20201221()
        {
            var countries = await _databaseService.GetCountries();
            foreach (var country in countries)
            {
                var flag = await GetFlagUriAsync(country.KyckrCountryCode, country.Iso2);
                if (flag != null)
                {
                    country.FlagUri = flag;
                }
                else
                {
                    country.FlagUri = null;
                }

                await((DatabaseService)_databaseService).UpdateCountry(country);
            }
        }

        public async Task<Uri> GetFlagUriAsync(string kyckrCountryCode, string countryCode)
        {
            try
            {
                await using var fs = new FileStream($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Migrations/Flags/{kyckrCountryCode}.png", FileMode.Open);
                var fileInfo = await _storageApiClient.PutFileAsync($"{kyckrCountryCode}.png", fs);
                return fileInfo.RetrievalUrl;
            }
            catch (Exception)
            {
                try
                {
                    await using var fs = new FileStream($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Migrations/Flags/{countryCode}.png", FileMode.Open);
                    var fileInfo = await _storageApiClient.PutFileAsync($"{countryCode}.png", fs);
                    return fileInfo.RetrievalUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unable to store seed data flag for {KyckrCountryCode} {CountryCode}", kyckrCountryCode, countryCode);
                }
            }

            return null;
        }
    }
}
