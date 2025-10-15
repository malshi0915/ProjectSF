using System.ComponentModel.DataAnnotations.Schema;

namespace BusBookingApi.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public Bus Bus { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public DateTime DepartureTime { get; set; }
        public decimal Price { get; set; }
        // Basic tracking coordinates (could be updated by a driver app)
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
    }
}
