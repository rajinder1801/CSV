using CSVProcessor.Contracts.Logging;
using CSVProcessor.Contracts.Repository;
using CSVProcessor.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace CSVProcessor.Service.Tests
{
    [TestClass()]
    public class CSVProcessorServiceTests
    {
        /// <summary>
        /// Repository object Mock;
        /// </summary>
        private Mock<ICSVProcessorRepository> _cSVProcessorRepositoryMock;

        /// <summary>
        /// Logger
        /// </summary>
        private Mock<ILogger> _logger;


        [TestInitialize]
        public void Setup()
        {
            _cSVProcessorRepositoryMock = new Mock<ICSVProcessorRepository>();
            _logger = new Mock<ILogger>();
        }

        [TestMethod()]
        public void GetDataTest_ReturnsValidData()
        {
            var expected = new List<SaleRecord>
            {
                {new  SaleRecord{ Country= "Australia", OrderDate = DateTime.Today, OrderID = 1, TotalCost = 100.00, UnitsSold = 1}}
            };
            _cSVProcessorRepositoryMock.Setup(x => x.GetData(It.IsAny<string>())).Returns(expected);


            var serviceObj = new CSVProcessorService(_cSVProcessorRepositoryMock.Object, _logger.Object);
            var actual = serviceObj.GetData("Test");
                                 
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDataTest_ExpectedException()
        {
            IEnumerable<SaleRecord> expected = null;
            _cSVProcessorRepositoryMock.Setup(x => x.GetData(It.IsAny<string>())).Returns(expected);

            var serviceObj = new CSVProcessorService(_cSVProcessorRepositoryMock.Object, _logger.Object);
            var actual = serviceObj.GetData("Test");
        }

        [TestMethod()]
        public void WriteDataTest_ValidData_ReturnsSuccess()
        {
            var expected = true;
            _cSVProcessorRepositoryMock.Setup(x => x.WriteData(It.IsAny<string>(),It.IsAny<string>())).Returns(expected);
            
            var serviceObj = new CSVProcessorService(_cSVProcessorRepositoryMock.Object, _logger.Object);
            var actual = serviceObj.WriteData("Test", "1,1,1,2,3,54");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void WriteDataTest_InValidData_ReturnsFailure()
        {
            var expected = false;
            _cSVProcessorRepositoryMock.Setup(x => x.WriteData(It.IsAny<string>(), It.IsAny<string>())).Returns(expected);

            var serviceObj = new CSVProcessorService(_cSVProcessorRepositoryMock.Object, _logger.Object);
            var actual = serviceObj.WriteData("Test", "asdasd");

            Assert.AreEqual(expected, actual);
        }


    }
}