using BusBookingApi.Models;

namespace BusBookingApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}