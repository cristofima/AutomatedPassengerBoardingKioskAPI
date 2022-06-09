using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomatedPassengerBoardingKiosk.API.Entities
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        [Required]
        [MaxLength(20)]
        public string Carrier { get; set; }

        [Required]
        [MaxLength(30)]
        public string From { get; set; }

        [Required]
        [MaxLength(30)]
        public string To { get; set; }

        [Required]
        public DateTime BoardingDate  { get; set; }

        [Required]
        [MaxLength(4)]
        public string Gate { get; set; }
    }
}
