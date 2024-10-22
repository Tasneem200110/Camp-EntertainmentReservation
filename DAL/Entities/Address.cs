using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Address
    {
        [Key] public int AddressId { get; set; }

        public string Government { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
    }
}