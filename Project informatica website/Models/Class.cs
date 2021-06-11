using System.ComponentModel.DataAnnotations;

namespace SendAndStore.Models
{
    public class Person 
    { 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Description { get; set; }

    }
}
