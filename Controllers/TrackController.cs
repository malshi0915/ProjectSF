using BusBookingApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TrackController(AppDbContext db) { _db = db; }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetTrack(int tripId)
        {
            var trip = await _db.Trips.FindAsync(tripId);
            if (trip == null) return NotFound();
            // Return current coords (in real system beta: driver app updates trip.CurrentLat/Lng)
            return Ok(new { tripId = trip.Id, lat = trip.CurrentLat, lng = trip.CurrentLng, departure = trip.DepartureTime });
        }
    }
}