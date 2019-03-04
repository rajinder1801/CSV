using CSVProcessor.Contracts.Logging;
using CSVProcessor.Contracts.Repository;
using CSVProcessor.Contracts.Service;
using System;
using System.Collections.Generic;

namespace CSVProcessor.Service
{
    /// <summary>
    /// Service Class
    /// </summary>
    public class CSVProcessorService : ICSVProcessorService
    {
        /// <summary>
        /// Repository object;
        /// </summary>
        private ICSVProcessorRepository  _cSVProcessorRepository;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="iCSVProcessorRepository"></param>
        public CSVProcessorService(ICSVProcessorRepository iCSVProcessorRepository, ILogger logger)
        {
            _cSVProcessorRepository = iCSVProcessorRepository?? throw new ArgumentNullException(nameof(iCSVProcessorRepository) + "is null");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger) + "is null.");
        }

        /// <summary>
        /// Gets the data from the mentioned csv file.
        /// </summary>
        /// <param name="csvName">Name of file</param>
        /// <returns></returns>
        public IEnumerable<object> GetData(string csvName)
        {
            IEnumerable<object> data;
            try
            {
                data = _cSVProcessorRepository.GetData(csvName);
                if (data == null)
                    throw new ArgumentNullException("CSV Data not found.");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }

        /// <summary>
        /// Sets the data in the mentioned csv file.
        /// </summary>
        /// <param name="csvName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteData(string csvName, string data)
        {
            var success = false;
            try
            {
                success = _cSVProcessorRepository.WriteData(csvName, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }
    }
}
