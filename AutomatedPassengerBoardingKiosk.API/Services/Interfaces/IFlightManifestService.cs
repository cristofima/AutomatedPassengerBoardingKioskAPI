using AutomatedPassengerBoardingKiosk.API.Entities;
using AutomatedPassengerBoardingKiosk.API.Responses;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AutomatedPassengerBoardingKiosk.API.Services.Interfaces
{
    public interface IFlightManifestService
    {
        BoardingPass ValidateFields(FormRecognizerResponse formRecognizerResult, VerifyResult faceDetectionResult);
    }
}
