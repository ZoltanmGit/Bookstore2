using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    public class Visitor : BaseUser
    {
        [Required]
        [MaxLength(50)]
        public String Address { get; set; }
    }
}
