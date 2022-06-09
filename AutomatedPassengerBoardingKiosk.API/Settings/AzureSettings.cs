namespace AutomatedPassengerBoardingKiosk.API.Settings
{
    public class AzureSettings
    {
        public FormRecognizer FormRecognizer { get; set; }
        public FaceDetection FaceDetection { get; set; }
        public VideoIndexer VideoIndexer { get; set; }
    }

    public class FormRecognizer
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ModelId { get; set; }
    }

    public class FaceDetection
    {
        public string ApiUrl { get; set; }
        public string SubscriptionKey { get; set; }
    }

    public class VideoIndexer
    {
        public string ApiUrl { get; set; }
        public string SubscriptionKey { get; set; }
        public string AccountId { get; set; }
        public string Location { get; set; }
    }
}
