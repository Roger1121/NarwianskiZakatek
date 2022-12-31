using NarwianskiZakatek.Models;
using NarwianskiZakatek.Services;
using NarwianskiZakatek.ViewModels;
using System.Web.Mvc;

namespace NarwianskiZakatek.Repositories
{
    public interface IReservationsService
    {
        Task AddReservation(RoomsViewModel roomList, string userName);
        void CancelReservation(Reservation reservation);
        List<SelectListItem> FindAvailableRooms(DateTime beginDate, DateTime endDate);
        Reservation GetReservation(string id);
        Task<PaginatedList<Reservation>> GetReservationsByParams(int? pageSize, int? pageNumber, string? sortOrder, DateTime? beginFrom, DateTime? beginTo, DateTime? endFrom, DateTime? endTo, decimal? priceFrom, decimal? priceTo, string? userId);
        List<Room> GetRoomsByReservation(Reservation reservation);
        void RateReservation(OpinionViewModel opinionViewModel, Reservation reservation);
    }
}