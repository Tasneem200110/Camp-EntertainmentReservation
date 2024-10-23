using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public String Email {  get; set; }
    }
}
