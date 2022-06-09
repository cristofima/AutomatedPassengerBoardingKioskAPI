using AutomatedPassengerBoardingKiosk.API.Contexts;
using AutomatedPassengerBoardingKiosk.API.Entities;
using AutomatedPassengerBoardingKiosk.API.Responses;
using AutomatedPassengerBoardingKiosk.API.Services.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AutomatedPassengerBoardingKiosk.API.Services
{
    public class FlightManifestService : IFlightManifestService
    {
        private readonly FlightManifestContext context;

        public FlightManifestService(FlightManifestContext context)
        {
            this.context = context;
        }

        public BoardingPass ValidateFields(FormRecognizerResponse formRecognizerResult, VerifyResult faceDetectionResult)
        {
            if (formRecognizerResult != null)
            {
                if (formRecognizerResult.IDCard != null && !string.IsNullOrEmpty(formRecognizerResult.IDCard.DocumentNumber))
                {
                    var person = context.People.Find(formRecognizerResult.IDCard.DocumentNumber);
                    if (person != null)
                    {
                        if (formRecognizerResult.BoardingPass != null)
                        {
                            var boardingPass = context.BoardingPasses.Include(x => x.Flight).Include(x => x.Passenger)
                                .Where(x => x.PassengerDocumentNumber == person.DocumentNumber && x.FlightNumber == formRecognizerResult.BoardingPass.FlightNumber)
                                .FirstOrDefault();
                            
                            if (boardingPass != null)
                            {
                                if (person.FirstName + " " + person.LastName == formRecognizerResult.BoardingPass.PassengerName)
                                {
                                    boardingPass.NameValidation = true;
                                }

                                var passenger = boardingPass.Passenger;
                                var flight = boardingPass.Flight;

                                string[] validformats = new[] { "MM/dd/yyyy", "MMM dd, yyyy - HH:mm" };
                                var provider = new CultureInfo("en-US");

                                var isBoardingDateTimeCorrect = false;

                                try
                                {
                                    var stringBoardingDateTime = formRecognizerResult.BoardingPass.Date + " - " + formRecognizerResult.BoardingPass.BoardingTime;
                                    var boardingDateTime = DateTime.ParseExact(stringBoardingDateTime, validformats, provider);
                                    if (boardingDateTime == flight.BoardingDate) isBoardingDateTimeCorrect = true;
                                }
                                catch (FormatException)
                                {
                                }

                                if (boardingPass.Class == formRecognizerResult.BoardingPass.Class && boardingPass.FlightNumber == formRecognizerResult.BoardingPass.FlightNumber
                                    && boardingPass.Seat == formRecognizerResult.BoardingPass.Seat && flight.Carrier == formRecognizerResult.BoardingPass.Carrier
                                    && flight.From == formRecognizerResult.BoardingPass.From && flight.To == formRecognizerResult.BoardingPass.To
                                    && isBoardingDateTimeCorrect)
                                {
                                    boardingPass.BoardingPassValidation = true;
                                }

                                if (faceDetectionResult != null && faceDetectionResult.IsIdentical)
                                {
                                    boardingPass.FaceValidation = true;
                                }

                                try
                                {
                                    var dateOfBirth = DateTime.ParseExact(formRecognizerResult.IDCard.DateOfBirth, validformats, provider);
                                    if(dateOfBirth.Date == person.Dob.Date)
                                    {
                                        boardingPass.DobValidation = true;
                                    }
                                }
                                catch (FormatException)
                                {
                                   
                                }

                                context.Update(boardingPass);
                                context.SaveChanges();

                                return boardingPass;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}