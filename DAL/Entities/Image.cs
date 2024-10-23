using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Source { get; set; }
        public int? CampId { get; set; }
        public Camp? Camp { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
