using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarBooking.API.Model
{
    public class Car
    {
        public int CarId { get; set; }

        [Required, MaxLength(20)]
        public string Brand { get; set; }

        [Required, MaxLength(20)]
        public string Model { get; set; }

        [Required, MaxLength(10)]
        public string LicensePlate { get; set; }


        public List<Booking> Bookings { get; set; }
    }
}
