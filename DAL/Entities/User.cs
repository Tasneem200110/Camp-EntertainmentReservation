using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            DateOfCreation = DateTime.Now;
            Bookings = new List<Booking>();
        }

        public DateTime DateOfCreation { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
