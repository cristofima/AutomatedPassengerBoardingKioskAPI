using AutomatedPassengerBoardingKiosk.API.Responses;

namespace AutomatedPassengerBoardingKiosk.API.Services.Interfaces
{
    public interface IFormRecognizerService
    {
        Task<FormRecognizerResponse> RecognizeDocuments(IFormFile idCard, IFormFile boardingPass);
    }
}
