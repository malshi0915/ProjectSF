using BusBookingApi.Data;
using BusBookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TripsController(AppDbContext db) { _db = db; }

        // GET api/trips?from=Colombo&to=Kandy&date=2025-10-12
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string from, [FromQuery] string to, [FromQuery] DateTime? date)
        {
            var q = _db.Trips.Include(t => t.Bus).Include(t => t.Route).AsQueryable();
            if (!string.IsNullOrEmpty(from)) q = q.Where(t => t.Route.FromCity.Contains(from));
            if (!string.IsNullOrEmpty(to)) q = q.Where(t => t.Route.ToCity.Contains(to));
            if (date.HasValue) q = q.Where(t => t.DepartureTime.Date == date.Value.Date);
            var list = await q.ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id}/seats")]
        public async Task<IActionResult> Seats(int id)
        {
            var trip = await _db.Trips.Include(t => t.Bus).FirstOrDefaultAsync(t => t.Id == id);
            if (trip == null) return NotFound();
            // ensure seat entries exist
            var seats = await _db.Seats.Where(s => s.TripId == id).ToListAsync();
            if (seats.Count == 0)
            {
                var total = trip.Bus.TotalSeats;
                for (int i = 1; i <= total; i++)
                {
                    _db.Seats.Add(new Seat { TripId = id, SeatNumber = i, IsBooked = false });
                }
                await _db.SaveChangesAsync();
                seats = await _db.Seats.Where(s => s.TripId == id).ToListAsync();
            }
            return Ok(seats.Select(s => new { s.SeatNumber, s.IsBooked }));
        }
    }
}