using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PalmEngineering.Models;

namespace PalmEngineering.Controllers
{
    public class HomeController : Controller
    {
        [AcceptVerbs("GET", "HEAD")]
        public IActionResult Index() => View(GetServices());

        [AcceptVerbs("GET", "HEAD")]
        public IActionResult About() => View();

        [AcceptVerbs("GET", "HEAD")]
        public IActionResult Services() => View(GetServices());

        [AcceptVerbs("GET", "HEAD")]
        public IActionResult Contact() => View();

        [AcceptVerbs("GET", "HEAD")]
        public IActionResult Gallery() => View(GalleryCatalog.Items);

        [AcceptVerbs("GET", "HEAD")]
        [Route("privacy")]
        public IActionResult Privacy() => View();

        [AcceptVerbs("GET", "HEAD")]
        [Route("terms")]
        public IActionResult Terms() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? code)
        {
            var status = code is >= 400 and < 600 ? code.Value : 500;
            Response.StatusCode = status;
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = status
            });
        }

        private static List<Service> GetServices() => new()
        {
            new Service { Name = "CNC Milling", Description = "High-precision CNC milling for complex parts with tight tolerances", Icon = "⚙️" },
            new Service { Name = "CNC Turning", Description = "Advanced lathe work for cylindrical components and rotational parts", Icon = "🔄" },
            new Service { Name = "Precision Grinding", Description = "Surface and cylindrical grinding for superior surface finish", Icon = "💎" },
            new Service { Name = "Wire EDM", Description = "Electrical discharge machining for intricate profiles and hard materials", Icon = "⚡" },
            new Service { Name = "Quality Inspection", Description = "Inspection and quality checks with documented results", Icon = "🔍" },
            new Service { Name = "Prototyping", Description = "Rapid prototyping services from concept to finished part", Icon = "🔧" }
        };
    }
}
