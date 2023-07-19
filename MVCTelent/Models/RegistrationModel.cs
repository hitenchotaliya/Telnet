namespace MVCTelent.Models
{
    public enum UserType
    {
        Candidate,
        Employer
    }

    public class RegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmEmail { get; set; }
        public string ActivationCode { get; set; }
        /*  public string ConfirmPassword { get; set; }*/
        public UserType UserType { get; set; }


    }
}
