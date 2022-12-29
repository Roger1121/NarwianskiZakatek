using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Repositories
{
    public interface IRoomsService
    {
        Task AddRoom(Room room);
        Task<bool> Delete(Room room);
        Task<Room> Get(int id);
        Task<List<Room>> GetRooms();
        bool RoomExists(int id);
        Task<string> UpdateRoom(Room room);
    }
}