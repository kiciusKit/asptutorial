using EmployeeMenagment.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMenagment.ViewModes
{
    public class EmploeeCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Display(Name = "Office Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
