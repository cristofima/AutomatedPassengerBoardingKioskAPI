namespace AutomatedPassengerBoardingKiosk.API.Models
{
    public class BoardingPassModel
    {
        public string BoardingTime { get; set; }
        public string Date { get; set; }
        public string Gate { get; set; }
        public int FlightNumber { get; set; }
        public string Carrier { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public char Class { get; set; }
        public string PassengerName { get; set; }
        public string Seat { get; set; }
    }
}
