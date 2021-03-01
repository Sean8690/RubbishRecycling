using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InfoTrack.Cdd.Application.Models;

namespace InfoTrack.Cdd.Application.Common.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface ITestablePersonClient
    {
        /// <summary>
        /// Check whether the service is available
        /// </summary>
        /// <returns></returns>
        Task<TestableClientResponse> PingAsync();

        /// <summary>
        /// 
        /// </summary>
        Task<TestableClientResponse> GetPersonsAsync<TRequest>(TRequest request);
    }
}
