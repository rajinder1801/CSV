using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSVProcessor.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using CSVProcessor.Contracts.Helper;
using CSVProcessor.Contracts.Logging;
using System.Data;
using CSVProcessor.Models;

namespace CSVProcessor.Repository.Tests
{
    [TestClass()]
    public class CSVProcessorRepositoryTests
    {
        /// <summary>
        /// Repository object Mock;
        /// </summary>
        private Mock<IHelper> _helperMock;

        /// <summary>
        /// Logger
        /// </summary>
        private Mock<ILogger> _logger;


        [TestInitialize]
        public void Setup()
        {
            _helperMock = new Mock<IHelper>();
            _logger = new Mock<ILogger>();
        }


        [TestMethod()]
        public void GetDataTest_InvalidFileName_ReturnsNull()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.GetData(null);

            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetDataTest_FileNotExists_ReturnsNull()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.GetData("Sample.csv");

            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void GetDataTest_ValidFileName_ReturnsValidData()
        {
            var expected = new Department { DeptId = 1};
            var dataTable = new DataTable("Sample");
            dataTable.Columns.Add(new DataColumn { ColumnName = "DeptId", DataType = typeof(int) });
            DataRow dr = dataTable.NewRow();
            dr[0] = 1;
            dataTable.Rows.Add(dr);

            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            _helperMock.Setup(x => x.GetDataTableFromCSVFile(It.IsAny<string>())).Returns(dataTable);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var data = obj.GetData("Departments.csv");

            var actual = data.FirstOrDefault() as Department;

            Assert.AreEqual(expected.DeptId, actual.DeptId);
        }


        [TestMethod()]
        public void WriteDataTest_FileNotExists_ReturnsFalse()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.WriteData("Sample.csv", null);

            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void WriteDataTest_InvalidFile_ReturnsFalse()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.WriteData(null, null);

            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void WriteDataTest_InvalidData_ReturnsFalse()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.WriteData("Sample.csv", null);

            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void WriteDataTest_ValidDataAndFile_ReturnsTrue()
        {
            _helperMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            _helperMock.Setup(x => x.AppendDataTofile(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var obj = new CSVProcessorRepository(_logger.Object, _helperMock.Object);
            var actual = obj.WriteData("Sample.csv", "1,1,1,2,3");

            Assert.IsTrue(actual);
        }




    }
}