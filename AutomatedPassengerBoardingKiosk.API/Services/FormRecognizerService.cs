using AutomatedPassengerBoardingKiosk.API.Models;
using AutomatedPassengerBoardingKiosk.API.Responses;
using AutomatedPassengerBoardingKiosk.API.Services.Interfaces;
using AutomatedPassengerBoardingKiosk.API.Settings;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.FormRecognizer.Models;

namespace AutomatedPassengerBoardingKiosk.API.Services
{
    public class FormRecognizerService : IFormRecognizerService
    {
        private AzureSettings azureSettings;
        private readonly AzureKeyCredential credential;

        public FormRecognizerService(AzureSettings azureSettings)
        {
            this.azureSettings = azureSettings;
            credential = new AzureKeyCredential(this.azureSettings.FormRecognizer.ApiKey);
        }

        public async Task<FormRecognizerResponse> RecognizeDocuments(IFormFile idCard, IFormFile boardingPass)
        {
            var documentClient = new DocumentAnalysisClient(new Uri(azureSettings.FormRecognizer.ApiUrl), credential);
            var documentOperation = await documentClient.StartAnalyzeDocumentAsync(azureSettings.FormRecognizer.ModelId, boardingPass.OpenReadStream());
            await documentOperation.WaitForCompletionAsync();

            var documentResult = documentOperation.Value;
            var response = new FormRecognizerResponse();

            var boardingPassModel = new BoardingPassModel();

            foreach (KeyValuePair<string, DocumentField> fieldKvp in documentResult.Documents[0].Fields)
            {
                var field = fieldKvp.Value;

                if (field != null)
                {
                    var fieldName = fieldKvp.Key;
                    if (fieldName == "Boarding Time")
                    {
                        boardingPassModel.BoardingTime = field.Content;
                    }
                    else if (fieldName == "Date")
                    {
                        boardingPassModel.Date = field.Content;
                    }
                    else if (fieldName == "Gate")
                    {
                        boardingPassModel.Gate = field.Content;
                    }
                    else if (fieldName == "Flight No.")
                    {
                        boardingPassModel.FlightNumber = int.Parse(field.Content);
                    }
                    else if (fieldName == "Carrier")
                    {
                        boardingPassModel.Carrier = field.Content;
                    }
                    else if (fieldName == "From")
                    {
                        boardingPassModel.From = field.Content;
                    }
                    else if (fieldName == "Class")
                    {
                        boardingPassModel.Class = field.Content[0];
                    }
                    else if (fieldName == "Passenger Name")
                    {
                        boardingPassModel.PassengerName = field.Content;
                    }
                    else if (fieldName == "To")
                    {
                        boardingPassModel.To = field.Content;
                    }
                    else if (fieldName == "Seat")
                    {
                        boardingPassModel.Seat = field.Content;
                    }
                }
            }

            var formClient = new FormRecognizerClient(new Uri(azureSettings.FormRecognizer.ApiUrl), credential);
            var formOperation = await formClient.StartRecognizeIdentityDocumentsAsync(idCard.OpenReadStream());
            await formOperation.WaitForCompletionAsync();

            var formResult = formOperation.Value;
            var idCardModel = new IDCardModel();

            foreach (KeyValuePair<string, FormField> fieldKvp in formResult[0].Fields)
            {
                var field = fieldKvp.Value;
                if (field != null && field.ValueData != null)
                {
                    var fieldName = fieldKvp.Key;
                    var content = field.ValueData.Text;
                    if (fieldName == "DateOfBirth")
                    {
                        idCardModel.DateOfBirth = content;
                    }
                    else if (fieldName == "DateOfExpiration")
                    {
                        idCardModel.DateOfExpiration = content;
                    }
                    else if (fieldName == "DocumentNumber")
                    {
                        idCardModel.DocumentNumber = content;
                    }
                    else if (fieldName == "FirstName")
                    {
                        idCardModel.FirstName = content;
                    }
                    else if (fieldName == "LastName")
                    {
                        idCardModel.LastName = content;
                    }
                    else if (fieldName == "Sex")
                    {
                        idCardModel.Sex = content[0];
                    }
                }
            }

            response.BoardingPass = boardingPassModel;
            response.IDCard = idCardModel;

            return response;
        }
    }
}