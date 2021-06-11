using System.ComponentModel.DataAnnotations;

namespace SendAndStore.Models
{
    public class Person 
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is needed")]
        [EmailAddress(ErrorMessage = "Not valid email address")]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Message needs to be filled out")]
        [Display(Name = "Message")]
        public string Description { get; set; }

    }
}
