using AutomatedPassengerBoardingKiosk.API.Models;

namespace AutomatedPassengerBoardingKiosk.API.Responses
{
    public class FormRecognizerResponse
    {
        public IDCardModel IDCard { get; set; }
        public BoardingPassModel BoardingPass { get; set; }
    }
}
