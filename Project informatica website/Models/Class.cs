using System.ComponentModel.DataAnnotations;

namespace SendAndStore.Models
{
    public class Person 
    { 
        [Required]
        public string firstname { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string country { get; set; }
        public string message { get; set; }

    }
}
