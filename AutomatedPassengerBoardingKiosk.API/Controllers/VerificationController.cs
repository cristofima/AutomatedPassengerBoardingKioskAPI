using AutomatedPassengerBoardingKiosk.API.Requests;
using AutomatedPassengerBoardingKiosk.API.Responses;
using AutomatedPassengerBoardingKiosk.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace AutomatedPassengerBoardingKiosk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IFormRecognizerService formRecognizerService;
        private readonly IFaceDetectionService faceDetectionService;
        private readonly IFlightManifestService flightManifestService;

        public VerificationController(IFormRecognizerService formRecognizerService, IFaceDetectionService faceDetectionService, IFlightManifestService flightManifestService)
        {
            this.formRecognizerService = formRecognizerService;
            this.faceDetectionService = faceDetectionService;
            this.flightManifestService = flightManifestService;
        }

        [HttpPost]
        public async Task<IActionResult> Verify([FromForm] FileRequest request)
        {
            FormRecognizerResponse formRecognizerResult = null;
            VerifyResult faceDetectionResult = null;

            try
            {
                formRecognizerResult = await formRecognizerService.RecognizeDocuments(request.IdCard, request.BoardingPass);
            }
            catch (Exception)
            {
            }

            try
            {
                faceDetectionResult = await faceDetectionService.VerifyFaces(request.IdCard, request.Video);
            }
            catch (Exception)
            {
            }

            var boardingPass = this.flightManifestService.ValidateFields(formRecognizerResult, faceDetectionResult);

            return Ok(new
            {
                formRecognizerResult,
                faceDetectionResult,
                boardingPass
            });
        }
    }
}