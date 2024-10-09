using System;
using MusicApp.Application.Interfaces;

namespace MusicApp.Application.Services;

public class EmailServices: IEmailService
{
   public Task SendEmailAsync(string email, string subject, string body)
   {
       // Implement email sending logic here
       Console.WriteLine($"Sending email to {email} with subject '{subject}' and body '{body}'");
       return Task.CompletedTask;
   }
}
