namespace BusBookingApi.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
        public int SeatNumber { get; set; }
        public bool IsBooked { get; set; }
    }
}