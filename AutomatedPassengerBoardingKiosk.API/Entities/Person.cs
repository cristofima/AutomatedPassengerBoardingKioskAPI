using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomatedPassengerBoardingKiosk.API.Entities
{
    public class Person
    {
        [Key]
        [MaxLength(15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string DocumentNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public char Sex { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public DateTime DocumentExpirationDate { get; set; }
    }
}
