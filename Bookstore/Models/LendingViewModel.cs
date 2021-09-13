using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class LendingViewModel
    {
        [DisplayName("VolumeID")]
        [Required]
        public int volumeID { get; set; }

        [DisplayName("StartDate")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }

        [DisplayName("StartDate")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
    }
}
