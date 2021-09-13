using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    /// <summary>
    /// Könyvek (cím, szerző, kiadás éve, ISBN szám, borítókép)
    /// </summary>
    public class Book
    {
        [Key]
        public String ISBN { get; set; }
        [Required]
        public String Title { get; set; }
        [Required]
        [MaxLength(50)]
        public String Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public byte[] CoverImage { get; set; }
    }
}
