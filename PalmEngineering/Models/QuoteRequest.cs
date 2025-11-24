using System.ComponentModel.DataAnnotations;

namespace PalmEngineering.Models
{
    public class QuoteRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Service type is required")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "Please describe your project")]
        public string ProjectDescription { get; set; }

        public string Material { get; set; }
        public int Quantity { get; set; }
        public DateTime? RequiredDate { get; set; }
    }
}