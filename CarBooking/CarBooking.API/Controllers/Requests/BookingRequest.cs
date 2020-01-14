using System;

namespace CarBooking.API.Controllers.Requests
{
    public class BookingRequest
    {
        public int CarId { get; set; }
        public DateTime BookingDate { get; set; }

        public bool IsValid()
        {
            return BookingDate != null && BookingDate.Date > DateTime.Now;
        }
    }
}
