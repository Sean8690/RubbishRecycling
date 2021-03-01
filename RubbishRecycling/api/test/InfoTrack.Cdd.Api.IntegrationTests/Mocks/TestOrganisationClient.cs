using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Api.IntegrationTests.Mocks
{
    public class TestOrganisationClient : ITestableOrganisationClient
    {
        private readonly HttpStatusCode _pingStatusCode;

        private readonly string _lookupFile;
        private readonly HttpStatusCode _lookupStatusCode;

        private readonly string _dataFile;
        private readonly HttpStatusCode _dataStatusCode;

        public TestOrganisationClient(string lookupFile, string dataFile,
            HttpStatusCode pingStatusCode = HttpStatusCode.OK,
            HttpStatusCode lookupStatusCode = HttpStatusCode.OK,
            HttpStatusCode dataStatusCode = HttpStatusCode.OK)
        {
            _lookupFile = lookupFile;
            _dataFile = dataFile;
            _pingStatusCode = pingStatusCode;
            _lookupStatusCode = lookupStatusCode;
            _dataStatusCode = dataStatusCode;
        }

        public async Task<TestableClientResponse> PingAsync()
        {
            return await Task.FromResult(
                new TestableClientResponse
                {
                    ResponseMessage = new HttpResponseMessage(_pingStatusCode),
                    ResponseString = "{\"commit\": \"7e7f818\",\"puppy\": true}"
                });
        }

        public async Task<TestableClientResponse> GetOrganisationsAsync<TRequest>(TRequest request)
        {
            return await Task.FromResult(
                new TestableClientResponse
                {
                    ResponseMessage = new HttpResponseMessage(_lookupStatusCode),
                    ResponseString = GetFileData(_lookupFile)
                });
        }

        public async Task<TestableClientResponse> GetOrganisationAsync<TRequest>(TRequest request)
        {
            return await Task.FromResult(
                new TestableClientResponse
                {
                    ResponseMessage = new HttpResponseMessage(_dataStatusCode),
                    ResponseString = GetFileData(_dataFile)
                });
        }

        private static string GetFileData(string fileName)
        {
            return "";

            //return File.ReadAllText($"E:/Git/InfoTrack.Cdd/api/test/InfoTrack.Cdd.Application.UnitTests/Data/{fileName}.json", Encoding.UTF8);

            // TODO get this actually working without hardcoding path (test data file is in the other test project)
            // This is a bit hacky because I set it up at the last minute to enable testing against live data without
            // continually making api calls. Should refactor when time later.
            // Should be moving the test data files to a shared project
            //return File.ReadAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}/Data/{fileName}.json", Encoding.UTF8);
        }
    }
}
