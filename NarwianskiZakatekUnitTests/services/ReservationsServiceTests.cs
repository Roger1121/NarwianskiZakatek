using Moq;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarwianskiZakatekUnitTests.services
{
    [TestClass]
    public class ReservationsServiceTests
    {
        public readonly MockData _mockData;

        public ReservationsServiceTests()
        {
            _mockData = new MockData();
        }

        [TestMethod]
        public void GetReservations_FilterByBeginDate()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);

            var result = service.GetReservationsByParams(10, 1, null, DateTime.Today.AddDays(-6), DateTime.Today, null, null, null, null, null);
            Assert.AreEqual(6, result.Result.Count);
        }

        [TestMethod]
        public void GetReservations_FilterByEndDate()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);

            var result = service.GetReservationsByParams(10, 1, null, null, null,DateTime.Today,DateTime.Today.AddDays(6), null, null, null);
            Assert.AreEqual(3, result.Result.Count);
        }

        [TestMethod]
        public void GetReservations_FilterByPrice()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);

            var result = service.GetReservationsByParams(10, 1, null, null, null, null, null, 250,2000, null);
            Assert.AreEqual(3, result.Result.Count);
        }

        [TestMethod]
        public void GetReservations_FilterByUserId()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);

            var result = service.GetReservationsByParams(10, 1, null, null, null, null, null, null, null, "4");
            Assert.AreEqual(2, result.Result.Count);
        }

        [TestMethod]
        public void GetReservation()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);
            var reservation = _mockData.reservations[0];
            var result = service.GetReservation("1");

            Assert.AreEqual(reservation.BeginDate, result.BeginDate);
            Assert.AreEqual(reservation.EndDate, result.EndDate);
            Assert.AreEqual(reservation.Price, result.Price);
            Assert.AreEqual(reservation.Opinion, result.Opinion);
            Assert.AreEqual(reservation.Rating, result.Rating);
        }

        [TestMethod]
        public void CancelReservation()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);
            var reservation = _mockData.reservations[3];
            service.CancelReservation(reservation);

            Assert.IsTrue(reservation.IsCancelled);
        }

        [TestMethod]
        public void RateReservation()
        {
            var mockSender = new Mock<IEmailService>().Object;
            var service = new ReservationsService(_mockData.Context.Object, mockSender);
            var reservation = _mockData.reservations[5];
            OpinionViewModel opinionViewModel = new OpinionViewModel() 
            { 
                Opinion = "opinion",
                Rating = 5
            };

            service.RateReservation(opinionViewModel, reservation);

            Assert.AreEqual(opinionViewModel.Opinion, reservation.Opinion);
            Assert.AreEqual(opinionViewModel.Rating, reservation.Rating);
        }
    }
}
