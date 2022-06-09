using System.ComponentModel.DataAnnotations;

namespace AutomatedPassengerBoardingKiosk.API.Requests
{
    public class FileRequest
    {
        [Required]
        public IFormFile IdCard { get; set; }

        [Required]
        public IFormFile BoardingPass { get; set; }

        [Required]
        public IFormFile Video { get; set; }
    }
}
