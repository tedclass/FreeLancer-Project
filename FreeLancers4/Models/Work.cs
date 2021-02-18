using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreeLancers4.Models
{
    public class Work
    {
        [Display(Name = "Project Number")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Enter a project name")]
        [DataType(DataType.Text)]
        [Display(Name = "Title")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Must start with a capital letter")]
        public string ProjectTitle { get; set; }


        [Display(Name = "Posted Date")]
        public DateTime PostDate { get; set; }


        [Required(ErrorMessage = "Enter a consice project description")]
        [DataType(DataType.Text)]
        [Display(Name = "Project description")]
        [StringLength(500, ErrorMessage = "Please write a decription of the project with as many details as you can manage", MinimumLength = 10)]
        public string Description { get; set; }


        [Required(ErrorMessage = "Enter the technologies you would prefer")]
        [DataType(DataType.Text)]
        [Display(Name = "Technologies Required")]
        public string Techneeded { get; set; }//technologies to be used for the project


        [Required(ErrorMessage = "Enter the skills you require")]
        [DataType(DataType.Text)]
        [Display(Name = "Skills Required")]
        public string Skills { get; set; }


        [Required(ErrorMessage = "What is the cost?")]
        [Range(1,9000, ErrorMessage = "The price cannot be more than 1000 USD!")]
        [Display(Name = "Project Cost")]
        public decimal Price { get; set; }
        

       
       public int TimeFrame { get; set; }//How long it might take
        
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "When is the project due?")]
        [Display(Name = "Deadline")]
        public DateTime DueDate { get; set; }//when is the product needed
        
        [Display(Name ="Contact Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please add an email you check frequently!")]
        public string contactEmail { get; set; }

    }
}
