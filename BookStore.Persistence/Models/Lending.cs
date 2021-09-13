using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Persistence.Models
{
    public class Lending
    {
        [Key]
        public int LendingId { get; set; }
        [Required]
        public virtual BookVolume Volume { get; set;}
        [Required]
        public virtual Visitor Visitor { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
