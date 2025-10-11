using BusBookingApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BusesController(AppDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buses = await _db.Buses.ToListAsync();
            return Ok(buses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bus = await _db.Buses.FindAsync(id);
            if (bus == null) return NotFound();
            return Ok(bus);
        }
    }
}