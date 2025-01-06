namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailto = "yash.0245113@gmail.com";
        private string _mailfrom = "yash.g.saini@gmail.com";

        public void Send(string subject, string message)
        {
            // send mail - output to debug window
            Console.WriteLine($"Mail from {_mailfrom} to {_mailto}, with LocalMailService.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
