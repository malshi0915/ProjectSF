namespace BusBookingApi.DTOs
{
    public class CreateBookingDto
    {
        public int TripId { get; set; }
        public int UserId { get; set; }
        public List<int> SeatNumbers { get; set; }
    }
}