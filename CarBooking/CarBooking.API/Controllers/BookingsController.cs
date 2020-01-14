using CarBooking.API.Controllers.Requests;
using CarBooking.API.Data;
using CarBooking.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CarBookingDataContext _context;

        public BookingsController(CarBookingDataContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.Include(booking => booking.Car).ToListAsync();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        [HttpPost]
        public async Task<ActionResult<Car>> PostBooking([FromBody] BookingRequest bookingRequest)
        {
            //
            // Check if request object is valid
            //
            if (!bookingRequest.IsValid())
            {
                return BadRequest("Invalid request object.");
            }

            //
            // Check if car exists
            //
            if (!_context.Cars.Any(car => car.CarId == bookingRequest.CarId))
            {
                return BadRequest("Invalid car id.");
            }

            //
            // Create new booking
            //
            _context.Bookings.Add(new Booking
            {
                CarId = bookingRequest.CarId,
                BookedDate = bookingRequest.BookingDate.Date
            });


            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Booking>> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return booking;
        }


        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
