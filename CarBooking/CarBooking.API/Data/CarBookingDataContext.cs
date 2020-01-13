using CarBooking.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CarBooking.API.Data
{
    public class CarBookingDataContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public CarBookingDataContext(DbContextOptions<CarBookingDataContext> options) : base(options)
        { }
    }
}
