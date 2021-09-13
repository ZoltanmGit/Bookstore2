using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    public class BookVolume
    {
        [Required]
        public virtual Book Book { get; set; }
        public virtual String BookISBN{get;set;}
        [Key]
        public Int32 LibraryId { get; set; }
    }
}
