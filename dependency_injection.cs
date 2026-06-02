using System;

namespace DependencyInjectionPractice
{
    public interface IMessageService
    {
        void SendMessage(string receiver, string message);
    }

    public class EmailService : IMessageService
    {
        public void SendMessage(string receiver, string message)
        {
            Console.WriteLine($"[Email Service] Sending email to {receiver}...");
            Console.WriteLine($"Body: \"{message}\"\n");
        }
    }

    public class SmsService : IMessageService
    {
        public void SendMessage(string receiver, string message)
        {
            Console.WriteLine($"[SMS Service] Sending SMS to {receiver}...");
            Console.WriteLine($"Text: \"{message}\"\n");
        }
    }

    public class NotificationManager
    {
        private readonly IMessageService _messageService;

        public NotificationManager(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void NotifyUser(string user, string alertMessage)
        {
            _messageService.SendMessage(user, alertMessage);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dependency Injection Practice\n");

            Console.WriteLine("- Testing Email Notification -");
            IMessageService emailService = new EmailService();
            NotificationManager emailNotification = new NotificationManager(emailService);
            emailNotification.NotifyUser("imran@gmail.com", "MVC lifecycle code is ready");

            Console.WriteLine("- Testing SMS Notification-");
            IMessageService smsService = new SmsService();
            NotificationManager smsNotification = new NotificationManager(smsService);
            smsNotification.NotifyUser("+123456789", "DI demonstration done");

            Console.ReadLine();
        }
    }
}