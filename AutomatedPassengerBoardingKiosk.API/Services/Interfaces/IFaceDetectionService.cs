using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AutomatedPassengerBoardingKiosk.API.Services.Interfaces
{
    public interface IFaceDetectionService
    {
        Task<VerifyResult> VerifyFaces(IFormFile idCard, IFormFile video);
    }
}
