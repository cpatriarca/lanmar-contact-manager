using System.ComponentModel.DataAnnotations;

namespace ContactManager.Application.Models
{
    public class ContactDto
    {
        public int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required string CompanyName { get; set; }

        [Phone]
        [Required]
        public required string Mobile { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}