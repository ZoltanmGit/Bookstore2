using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookstore.Persistence.Models
{
    public class BaseUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
    }
}
