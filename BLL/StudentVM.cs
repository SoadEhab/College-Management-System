using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DataModel;

namespace BLL
{
    public class StudentVM
    {
        [Key]
        public int studentId { get; set; }
        [Required (ErrorMessage ="Your Level is required")]
        public Nullable<int> levelId { get; set; }
        [Required(ErrorMessage = "Your NationalID is required")]
        public decimal nationalId { get; set; }
        [Required(ErrorMessage = "Your Mobile is required")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Your Password is required")]
        public string password { get; set; }
        [Required(ErrorMessage = "Your First Name is required")]
        public string firstName { get; set; }
        [Required(ErrorMessage = "Your Middel Name is required")]
        public string middelName { get; set; }
        [Required(ErrorMessage = "Your Last Name is required")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Your Sur Name is required")]
        public string surName { get; set; }
        [Required(ErrorMessage = "Your Department is required")]
        public Nullable<int> deptId { get; set; }
        [Required(ErrorMessage = "Your Gender is required")]
        public Nullable<int> genderId { get; set; }
        public string image { get; set; }

       
    }
}
