using BusBookingApi.Data;
using BusBookingApi.DTOs;
using BusBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BookingsController(AppDbContext db) { _db = db; }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var trip = await _db.Trips.Include(t => t.Bus).FirstOrDefaultAsync(t => t.Id == dto.TripId);
            if (trip == null) return BadRequest("Trip not found.");
            var seats = await _db.Seats.Where(s => s.TripId == dto.TripId && dto.SeatNumbers.Contains(s.SeatNumber)).ToListAsync();
            if (seats.Any(s => s.IsBooked)) return BadRequest("One or more seats already booked.");

            // Mark seats booked
            foreach (var s in seats) s.IsBooked = true;

            var booking = new Booking
            {
                TripId = dto.TripId,
                UserId = dto.UserId,
                SeatNumbers = dto.SeatNumbers,
                TotalPrice = trip.Price * dto.SeatNumbers.Count
            };
            _db.Bookings.Add(booking);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _db.Bookings.Include(b => b.Trip).ThenInclude(t => t.Route).Include(b => b.User).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }
    }
}