using TG_Task3.Controllers;
using TG_Task3.Model;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Globalization;

namespace TG_Task3.Tests
{
    public class Tests
    {
        private int mockCorrectHotelId;
        private HotelRatesController hotelRatesController;
        private DateTime mockCorrectArrivalDate;

        [SetUp]
        public void Setup()
        {
            hotelRatesController = new HotelRatesController();
            mockCorrectHotelId = 7294;
            mockCorrectArrivalDate = DateTime.ParseExact("2016-03-15", "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        [Test]
        public void Get_WithWrongHotelId_ReturnsNotFoundResult()
        {
            //Arrange
            int invalidHotelId = 1;

            // Act
            var notFoundResult = hotelRatesController.GetHotelRatesList(invalidHotelId, DateTime.Now);

            var resultObject = (notFoundResult as NotFoundObjectResult).Value;

            Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);
            Assert.AreEqual(invalidHotelId, resultObject.GetType().GetProperty("HotelId").GetValue(resultObject));
        }

        [Test]
        public void Get_WithValidHotelIdWrongArrivalDate_ReturnsNotFoundResult()
        {
            DateTime invalidArrivalDate = DateTime.Now;

            // Act
            var notFoundResult = hotelRatesController.GetHotelRatesList(mockCorrectHotelId, invalidArrivalDate);
            var resultObject = (notFoundResult as NotFoundObjectResult).Value;

            // extracting values from the anonymous types
            var resultType = resultObject.GetType();
            var resultHotelId = resultType.GetProperty("HotelId").GetValue(resultObject);
            var resultArrivalDate = resultType.GetProperty("ArrivalDate").GetValue(resultObject);

            Assert.IsInstanceOf<NotFoundObjectResult>(notFoundResult);
            Assert.AreEqual(mockCorrectHotelId, resultHotelId);
            Assert.AreEqual(invalidArrivalDate, resultArrivalDate);
        }

        [Test]
        public void Get_WithValidHotelIdAndArrivalDate_ReturnsOkResult()
        {
            var okResult = hotelRatesController.GetHotelRatesList(mockCorrectHotelId, mockCorrectArrivalDate);
            var okResultObject = (okResult as OkObjectResult).Value;

            Assert.IsInstanceOf<OkObjectResult>(okResult);
            Assert.IsInstanceOf<TGHotelRate>(okResultObject);

            TGHotelRate tgHotelRateResult = okResultObject as TGHotelRate;

            // Verifying the returned list contains valid results
            Assert.AreEqual(mockCorrectHotelId, tgHotelRateResult.Hotel.HotelID);
            Assert.That(tgHotelRateResult.HotelRates.TrueForAll(x => x.TargetDay.Date == mockCorrectArrivalDate));
        }
    }
}