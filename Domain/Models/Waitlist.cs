using Domain.Requests;
using System;

namespace Domain.Models
{
    public class Waitlist
    {
        //EF requires empty ctor for model binding
        public Waitlist()
        {
        }

        public Waitlist(CreateWaitlistRequest request)
        {
            CustomerId = request.CustomerId;
            PartySize = request.PartySize;
            CreatedDateTime = DateTime.Now;
            IsNotified = false;
            IsActive = true;
        }

        public int Id { get; set; }
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }
        public int PartySize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsNotified { get; set; }
        public bool IsActive { get; set; }
    }
}
