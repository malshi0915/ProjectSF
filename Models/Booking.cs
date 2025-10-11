using BusBookingApi.Models;


public class Booking
{
    public int Id { get; set; }
    public int TripId { get; set; }
    public Trip Trip { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<int> SeatNumbers { get; set; }
    public DateTime BookingTime { get; set; }
    public decimal TotalPrice { get; set; }
}
