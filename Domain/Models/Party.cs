using Domain.Requests;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Party
    {
        public static readonly Dictionary<string, string> PropNameToPatchPath = new Dictionary<string, string>
        {
            [nameof(Party.IsActive)] = "/isActive",
            [nameof(Party.IsNotified)] = "/isNotified",
        };

        //EF requires empty ctor for model binding
        public Party()
        {
        }

        public Party(CreatePartyRequest request)
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
