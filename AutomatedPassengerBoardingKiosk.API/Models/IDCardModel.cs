namespace AutomatedPassengerBoardingKiosk.API.Models
{
    public class IDCardModel
    {
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfExpiration { get; set; }
        public char Sex { get; set; }
    }
}
