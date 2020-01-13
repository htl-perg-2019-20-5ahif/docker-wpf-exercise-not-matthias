using System;

namespace CarBooking.API.Controllers.Requests
{
    public class BookingRequest
    {
        public int CarId { get; set; }
        public DateTime BookedDate { get; set; }

        public bool IsValid()
        {
            return BookedDate != null && BookedDate.Date > DateTime.Now;
        }
    }
}
