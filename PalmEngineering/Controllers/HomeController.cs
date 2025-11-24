using Microsoft.AspNetCore.Mvc;
using PalmEngineering.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace PrecisionEngineering.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var services = GetServices();
            return View(services);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            var services = GetServices();
            return View(services);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(QuoteRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Send email notification
                    SendQuoteEmail(request);

                    TempData["SuccessMessage"] = "Thank you! We've received your quote request and will contact you within 24 hours.";
                    return RedirectToAction("Contact");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "There was an error submitting your request. Please call us at 082 921 7644 or email palmeng@netsurfers.co.za";
                    return View(request);
                }
            }
            return View(request);
        }

        private void SendQuoteEmail(QuoteRequest request)
        {
            // Email configuration
            string toEmail = "palmeng@netsurfers.co.za";
            string subject = $"New Quote Request from {request.Company}";

            string body = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>New Quote Request</h2>
                    <table style='border-collapse: collapse; width: 100%;'>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Name:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Name}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Company:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Company}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Email:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Email}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Phone:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Phone}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Service Type:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.ServiceType}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Material:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Material}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Quantity:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.Quantity}</td></tr>
                        <tr><td style='padding: 8px; border: 1px solid #ddd;'><strong>Required Date:</strong></td><td style='padding: 8px; border: 1px solid #ddd;'>{request.RequiredDate?.ToString("dd/MM/yyyy") ?? "Not specified"}</td></tr>
                    </table>
                    <h3>Project Description:</h3>
                    <p style='border: 1px solid #ddd; padding: 10px; background: #f9f9f9;'>{request.ProjectDescription}</p>
                    <p style='color: #666; margin-top: 20px;'>Submitted on: {DateTime.Now:dd/MM/yyyy HH:mm}</p>
                </body>
                </html>
            ";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("noreply@palmengineering.co.za");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.ReplyToList.Add(new MailAddress(request.Email, request.Name));

                // NOTE: Configure your SMTP settings in appsettings.json
                // For now, this will log the quote request
                System.IO.File.AppendAllText("quote_requests.txt",
                    $"\n\n=== Quote Request {DateTime.Now} ===\n" +
                    $"Name: {request.Name}\n" +
                    $"Company: {request.Company}\n" +
                    $"Email: {request.Email}\n" +
                    $"Phone: {request.Phone}\n" +
                    $"Service: {request.ServiceType}\n" +
                    $"Material: {request.Material}\n" +
                    $"Quantity: {request.Quantity}\n" +
                    $"Required Date: {request.RequiredDate}\n" +
                    $"Description: {request.ProjectDescription}\n"
                );
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Service> GetServices()
        {
            return new List<Service>
            {
                new Service { Id = 1, Name = "CNC Milling", Description = "High-precision CNC milling for complex parts with tight tolerances up to ±0.001\"", IconClass = "⚙️" },
                new Service { Id = 2, Name = "CNC Turning", Description = "Advanced lathe work for cylindrical components and rotational parts", IconClass = "🔄" },
                new Service { Id = 3, Name = "Precision Grinding", Description = "Surface and cylindrical grinding for superior surface finish", IconClass = "💎" },
                new Service { Id = 4, Name = "Wire EDM", Description = "Electrical discharge machining for intricate profiles and hard materials", IconClass = "⚡" },
                new Service { Id = 5, Name = "Quality Inspection", Description = "CMM inspection and quality assurance with detailed reporting", IconClass = "🔍" },
                new Service { Id = 6, Name = "Prototyping", Description = "Rapid prototyping services from concept to finished part", IconClass = "🔧" }
            };
        }
    }
}