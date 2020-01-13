using System;
using System.ComponentModel.DataAnnotations;

namespace CarBooking.API.Model
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public DateTime Date { get; set; }


        [Required]
        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
