using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomatedPassengerBoardingKiosk.API.Entities
{
    public class BoardingPass
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string PassengerDocumentNumber { get; set; }

        [Required]
        public int FlightNumber { get; set; }

        [Required]
        public Person Passenger { get; set; }

        [Required]
        public Flight Flight { get; set; }

        [Required]
        [MaxLength(4)]
        public string Seat { get; set; }

        [Required]
        public char Class { get; set; }

        public bool NameValidation { get; set; }
        public bool DobValidation { get; set; }
        public bool FaceValidation { get; set; }
        public bool BoardingPassValidation { get; set; }
    }
}
