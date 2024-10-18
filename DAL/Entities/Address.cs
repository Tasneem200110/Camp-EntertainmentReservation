using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string Government { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }
}
