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
<<<<<<< Updated upstream
=======
        [Route("cookies")]
        public IActionResult Cookies() => View();

        [AcceptVerbs("GET", "HEAD")]
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            new Service { Name = "CNC Milling", Description = "High-precision CNC milling for complex parts with tight tolerances", Icon = "⚙️" },
            new Service { Name = "CNC Turning", Description = "Advanced lathe work for cylindrical components and rotational parts", Icon = "🔄" },
            new Service { Name = "Precision Grinding", Description = "Surface and cylindrical grinding for superior surface finish", Icon = "💎" },
            new Service { Name = "Wire EDM", Description = "Electrical discharge machining for intricate profiles and hard materials", Icon = "⚡" },
            new Service { Name = "Quality Inspection", Description = "Inspection and quality checks with documented results", Icon = "🔍" },
            new Service { Name = "Prototyping", Description = "Rapid prototyping services from concept to finished part", Icon = "🔧" }
=======
            new Service { Id = 1, Name = "CNC Milling", Description = "High-precision CNC milling for complex parts with tight tolerances", IconClass = "mill" },
            new Service { Id = 2, Name = "CNC Turning", Description = "Advanced lathe work for cylindrical components and rotational parts", IconClass = "turn" },
            new Service { Id = 3, Name = "Precision Grinding", Description = "Surface and cylindrical grinding for superior surface finish", IconClass = "grind" },
            new Service { Id = 4, Name = "Wire EDM", Description = "Electrical discharge machining for intricate profiles and hard materials", IconClass = "bolt" },
            new Service { Id = 5, Name = "Quality Inspection", Description = "Inspection and quality checks with documented results", IconClass = "inspect" },
            new Service { Id = 6, Name = "Prototyping", Description = "Rapid prototyping services from concept to finished part", IconClass = "cube" }
>>>>>>> Stashed changes
        };
    }
}
