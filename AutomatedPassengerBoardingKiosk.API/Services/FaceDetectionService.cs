using AutomatedPassengerBoardingKiosk.API.Services.Interfaces;
using AutomatedPassengerBoardingKiosk.API.Settings;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Newtonsoft.Json;
using System.Net;

namespace AutomatedPassengerBoardingKiosk.API.Services
{
    public class FaceDetectionService: IFaceDetectionService
    {
        private readonly IFaceClient faceClient;

        private AzureSettings azureSettings;

        public FaceDetectionService(AzureSettings azureSettings)
        {
            this.azureSettings = azureSettings;
            faceClient = new FaceClient(new ApiKeyServiceClientCredentials(this.azureSettings.FaceDetection.SubscriptionKey)) { Endpoint = this.azureSettings.FaceDetection.ApiUrl };
        }

        public async Task<VerifyResult> VerifyFaces(IFormFile idCard, IFormFile video)
        {
            var facesFromIDCard = await faceClient.Face.DetectWithStreamAsync(idCard.OpenReadStream());
            if(facesFromIDCard != null && facesFromIDCard.Count > 0 && facesFromIDCard[0].FaceId != null)
            {
                var faceId1 = facesFromIDCard[0].FaceId.Value;
                var thumbnailUrl = AnalizeVideo(video);
                var facesFromVideo = await faceClient.Face.DetectWithUrlAsync(thumbnailUrl);
                if (facesFromVideo != null && facesFromVideo.Count > 0 && facesFromVideo[0].FaceId != null)
                {
                    return await faceClient.Face.VerifyFaceToFaceAsync(faceId1, facesFromVideo[0].FaceId.Value);
                }

                throw new Exception("No faces detected in Video");
            }

            throw new Exception("No faces detected in ID Card");
        }

        private string AnalizeVideo(IFormFile video)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;

            // create the http client
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureSettings.VideoIndexer.SubscriptionKey);

            // obtain account access token
            var accountAccessTokenRequestResult = client.GetAsync($"{azureSettings.VideoIndexer.ApiUrl}/auth/{azureSettings.VideoIndexer.Location}/Accounts/{azureSettings.VideoIndexer.AccountId}/AccessToken?allowEdit=true").Result;
            var accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-KeWy");

            // upload a video
            var content = new MultipartFormDataContent();

            using(var ms = new MemoryStream())
            {
                video.CopyTo(ms);
                content.Add(new ByteArrayContent(ms.ToArray()), "body", "videotest.mp4");
            }

            var videoName = Guid.NewGuid();

            var uploadRequestResult = client.PostAsync($"{azureSettings.VideoIndexer.ApiUrl}/{azureSettings.VideoIndexer.Location}/Accounts/{azureSettings.VideoIndexer.AccountId}/Videos?accessToken={accountAccessToken}&name={videoName}&privacy=private", content).Result;
            var uploadResult = uploadRequestResult.Content.ReadAsStringAsync().Result;

            // get the video id from the upload result
            var videoId = JsonConvert.DeserializeObject<dynamic>(uploadResult)["id"];

            // obtain video access token
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureSettings.VideoIndexer.SubscriptionKey);
            var videoTokenRequestResult = client.GetAsync($"{azureSettings.VideoIndexer.ApiUrl}/auth/{azureSettings.VideoIndexer.Location}/Accounts/{azureSettings.VideoIndexer.AccountId}/Videos/{videoId}/AccessToken?allowEdit=true").Result;
            var videoAccessToken = videoTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

            client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

            // wait for the video index to finish
            while (true)
            {
                Thread.Sleep(10000);

                var videoGetIndexRequestResult = client.GetAsync($"{azureSettings.VideoIndexer.ApiUrl}/{azureSettings.VideoIndexer.Location}/Accounts/{azureSettings.VideoIndexer.AccountId}/Videos/{videoId}/Index?accessToken={videoAccessToken}&language=English").Result;
                var videoGetIndexResult = videoGetIndexRequestResult.Content.ReadAsStringAsync().Result;

                var indexedVideoInfo = JsonConvert.DeserializeObject<dynamic>(videoGetIndexResult);

                var processingState = indexedVideoInfo["state"];

                // job is finished
                if (processingState == "Processed")
                {
                    var thumbnailId = indexedVideoInfo["videos"][0]["insights"]["faces"][0]["thumbnails"][0]["id"];
                    var url = $"{azureSettings.VideoIndexer.ApiUrl}/{azureSettings.VideoIndexer.Location}/Accounts/{azureSettings.VideoIndexer.AccountId}/Videos/{videoId}/Thumbnails/{thumbnailId}?format=Jpeg&accessToken={videoAccessToken}";

                    return url;
                }
            }
        }
    }
}