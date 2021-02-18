using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeLancers4.Models
{
    public class History
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Enter the developers username!")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Required]
        [Display(Name = "Enter their proffesion")]
        public string Proffesion { get; set; }

        [DataType(DataType.Text)]
        [Required]
        [Display(Name = "What was the projects name")]
        public string Project { get; set; }

        [Range(1,5,ErrorMessage = "Please enter a number betwen 1 and 5")]
        [Required]
        public int Rating { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime completeDate { get; set; }
        
    }
}
