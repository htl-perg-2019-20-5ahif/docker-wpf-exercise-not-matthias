using System;
using System.ComponentModel.DataAnnotations;

namespace CarBooking.API.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime BookedDate { get; set; }


        public int CarId { get; set; }

        [Required]
        public Car Car { get; set; }
    }
}
