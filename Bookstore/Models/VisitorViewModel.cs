using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class VisitorLoginViewModel
    {
        [DisplayName("Username")]
        [Required]
        public String visitorUserName { get; set; }
        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public String visitorPassword { get; set; }
    }
    public class VisitorRegistrationViewModel
    {
        [DisplayName("Username")]
        [Required]
        public string visitorUserName { get; set; }
        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string visitorPassword { get; set; }

        [DisplayName("Confirmation Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("visitorPassword")]
        public string visitorConfirmationPassword { get; set; }
        [DisplayName("Name")]
        [Required]
        public string visitorName { get; set; }
        [DisplayName("Address")]
        [Required]
        public string visitorAddress { get; set; }
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string visitorPhoneNumber { get; set; }
    }
}
