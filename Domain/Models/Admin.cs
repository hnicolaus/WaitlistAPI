﻿namespace Domain.Models
{
    public class Admin
    {
        public Admin()
        {

        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticationCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}