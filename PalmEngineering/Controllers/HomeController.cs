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

        public IActionResult Gallery()
        {
            // Sample data - replace with database or actual image paths
            var galleryItems = new List<GalleryItem>
            {
            new GalleryItem
        {
            Id = 1,
            Title = "Assembly Fixtures",
            Description = "Custom-designed assembly fixtures for manufacturing processes",
            ImageUrl = "/images/gallery/assembly-fixtures.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 2,
            Title = "Wheel Spacers",
            Description = "Precision-machined wheel spacers for automotive applications",
            ImageUrl = "/images/gallery/wheel-spacers.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 3,
            Title = "Custom Conveyor Rollers",
            Description = "Engineered conveyor rollers for industrial material handling systems",
            ImageUrl = "/images/gallery/conveyor-rollers.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 4,
            Title = "Stainless Steel Fabrications",
            Description = "High-quality stainless steel fabrication work for various industries",
            ImageUrl = "/images/gallery/stainless-fabrications.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 5,
            Title = "Special Purpose Manifolds",
            Description = "Custom-designed manifolds for specialized applications",
            ImageUrl = "/images/gallery/manifolds.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 6,
            Title = "Educational Projects",
            Description = "Precision components manufactured for educational institutions",
            ImageUrl = "/images/gallery/educational-projects.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 7,
            Title = "Lathe Work",
            Description = "Expert precision turning and lathe machining services",
            ImageUrl = "/images/gallery/lathe-work.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 8,
            Title = "Stainless Steel Fittings",
            Description = "Precision-machined stainless steel fittings and connections",
            ImageUrl = "/images/gallery/stainless-fittings.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 9,
            Title = "Woodwork Limit Clamps",
            Description = "Specialized limit clamps for woodworking applications",
            ImageUrl = "/images/gallery/woodwork-clamps.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 10,
            Title = "Precision Turning",
            Description = "High-precision CNC turning for demanding specifications",
            ImageUrl = "/images/gallery/precision-turning.jpg",
            DateAdded = DateTime.Now
        },
        new GalleryItem
        {
            Id = 11,
            Title = "Malt Cracking Mills",
            Description = "Custom-engineered malt cracking mills for brewing applications",
            ImageUrl = "/images/gallery/malt-mills.jpg",
            DateAdded = DateTime.Now
        }
    };

            return View(galleryItems);
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