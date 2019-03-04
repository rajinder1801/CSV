using CSVProcessor.Contracts.Helper;
using CSVProcessor.Contracts.Logging;
using CSVProcessor.Contracts.Repository;
using CSVProcessor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CSVProcessor.Repository
{

    /// <summary>
    /// Repository Class
    /// </summary>
    public class CSVProcessorRepository : ICSVProcessorRepository
    {
        /// <summary>
        /// Base path
        /// </summary>
        private readonly string _csvBasePath;

        /// <summary>
        /// Logger
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Helper
        /// </summary>
        private IHelper _helper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public CSVProcessorRepository(ILogger logger, IHelper helper)
        {
            _csvBasePath = ConfigurationManager.AppSettings["BasePath"] ?? throw new ArgumentException("Base Path is not defined.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger) + "is null.");
            _helper = helper ?? throw new ArgumentNullException(nameof(helper) + "is null.");
        }

        /// <summary>
        /// Reads, Parses and returns a type list from a csv.
        /// </summary>
        /// <param name="csvName">Name of the csv file.</param>
        /// <returns>Data list.</returns>
        public IEnumerable<object> GetData(string csvName)
        {
            var filePath = string.Concat(_csvBasePath, csvName);
            IList<object> data = null;

            if (string.IsNullOrEmpty(csvName) || !_helper.FileExists(filePath))
                return data;

            try
            {
                data = new List<object>();
                Type objType = GetClassTypeForFile(csvName); 
                if (objType != null)
                {
                    //Get the properties for the class object type
                    PropertyInfo[] filefields;
                    filefields = objType.GetProperties();
                    //Get the data from the CSV file and populate the object listDataTable 
                    var tmpTable = _helper.GetDataTableFromCSVFile(filePath);
                    foreach (DataRow dr in tmpTable.Rows)
                    {
                        object tmpObj = Activator.CreateInstance(objType);
                        foreach (PropertyInfo pinfo in filefields)
                        {
                            var propertyName = pinfo.Name.Trim().ToLower();
                            //check to see if the table has a column with the specified field name
                            if (dr.Table.Columns.Contains(propertyName))
                            {
                                //set the value of the object's property
                                pinfo.SetValue(tmpObj, GetValue(dr[propertyName].ToString(), pinfo.PropertyType), null);
                            }
                        }

                        //add the object to the data list.
                        data.Add(tmpObj);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception : ", ex);
                throw new Exception("Error Occurred. Please Try Again.");
            }

            return data;
        }

        /// <summary>
        /// Writes the data supplied to the csv.
        /// </summary>
        /// <param name="csvName">Name of the csv file.</param>
        /// <param name="data">The data to be written to the csv.</param>
        /// <returns>A boolean value.</returns>
        public bool WriteData(string csvName, string data)
        {
            var filePath = string.Concat(_csvBasePath, csvName);
            var success = false;

            if (string.IsNullOrEmpty(csvName) || !_helper.FileExists(filePath))
                return success;

            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    success = _helper.AppendDataTofile(filePath, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception : ", ex);
                throw new Exception("Error Occurred. Please Try Again.");
            }

            return success;
        }


        #region PrivateMethods

        /// <summary>
        /// Gets a specific value from a type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private object GetValue(string value, Type propertyType)
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyType);
            return typeConverter.ConvertFromString(value);
        }

        /// <summary>
        /// Get class type mapping for a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Type GetClassTypeForFile(string fileName)
        {
            switch (fileName)
            {
                case "SaleRecords.csv":
                    return typeof(SaleRecord);
                case "Departments.csv":
                    return typeof(Department);
                default:
                    return typeof(object);
            }
        }

       

        #endregion
    }
}
