# Palm Engineering - Professional CNC Machining Website

A modern, responsive ASP.NET Core MVC website for Palm Engineering, a precision CNC machining and manufacturing company based in Gqeberha, South Africa.

![Palm Engineering](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## 🚀 Features

- **Responsive Design** - Fully mobile-friendly and tablet-optimized
- **Modern UI/UX** - Clean, professional design with smooth animations
- **Quote Request System** - Functional contact form with validation
- **Service Showcase** - Detailed information about CNC machining services
- **About Section** - Company information and capabilities
- **Full-Width Layout** - Optimized for modern displays

## 🛠️ Technologies Used

- **ASP.NET Core 6.0+** - Modern web framework
- **C# MVC Pattern** - Model-View-Controller architecture
- **Razor Pages** - Dynamic HTML generation
- **CSS3** - Modern styling with CSS variables
- **Inter Font** - Professional Google Font
- **Form Validation** - Built-in ASP.NET Core validation

## 📋 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download) or later
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

## 🔧 Installation

1. **Clone the repository**
```bash
   git clone https://github.com/yourusername/palm-engineering.git
   cd palm-engineering
```

2. **Restore dependencies**
```bash
   dotnet restore
```

3. **Build the project**
```bash
   dotnet build
```

4. **Run the application**
```bash
   dotnet run
```

5. **Open in browser**
   Navigate to `https://localhost:5001` or `http://localhost:5000`

## 📁 Project Structure
```
PalmEngineering/
├── Controllers/
│   └── HomeController.cs          # Main application controller
├── Models/
│   ├── Service.cs                 # Service data model
│   ├── QuoteRequest.cs            # Quote form model
│   └── ErrorViewModel.cs          # Error handling model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Homepage
│   │   ├── About.cshtml           # About page
│   │   ├── Services.cshtml        # Services page
│   │   └── Contact.cshtml         # Contact/Quote page
│   ├── Shared/
│   │   ├── _Layout.cshtml         # Main layout template
│   │   └── Error.cshtml           # Error page
│   ├── _ViewImports.cshtml        # Global view imports
│   └── _ViewStart.cshtml          # View configuration
├── wwwroot/                       # Static files (if needed)
├── Program.cs                     # Application entry point
└── appsettings.json               # Configuration settings
```

## 🎨 Pages Overview

### Home Page
- Hero section with call-to-action buttons
- Services overview grid (6 services)
- Statistics section (Years, Precision, Projects, Certification)
- Why Choose Us section

### About Page
- Company history and mission
- Detailed capabilities list
- Quality standards and certifications
- Industries served

### Services Page
- Comprehensive service descriptions
- CNC Milling, Turning, Grinding, Wire EDM
- Quality Control and Prototyping
- Materials we work with

### Contact Page
- Fully functional quote request form
- Form validation
- Contact information (phone, email, address)
- Business hours

## 📞 Contact Information

**Palm Engineering**
- **Address:** 242 Commercial Rd, Sidwell, Gqeberha, 6070
- **Preferred Phone:** 082 921 7644
- **Alternative Phone:** 083 656 6604
- **Work Phone:** 041 453 9681
- **Email:** palmeng@netsurfers.co.za
- **Hours:** Monday-Friday: 8:00 AM - 5:00 PM

## 🔒 Quote Request Form

The contact form captures:
- Customer name and company
- Email and phone number
- Service type selection
- Material specifications
- Quantity requirements
- Required delivery date
- Detailed project description

All submissions are saved to `quote_requests.txt` in the project root directory.

## 🎨 Customization

### Changing Colors

Edit the CSS variables in `Views/Shared/_Layout.cshtml`:
```css
:root {
    --primary: #0f2557;           /* Main brand color */
    --primary-dark: #0a1a3d;      /* Darker shade */
    --secondary: #2563eb;          /* Accent color */
    --accent: #3b82f6;            /* Secondary accent */
}
```

### Adding New Services

Edit `Controllers/HomeController.cs` in the `GetServices()` method:
```csharp
new Service { 
    Id = 7, 
    Name = "New Service", 
    Description = "Service description", 
    IconClass = "🔧" 
}
```

## 📧 Email Configuration (Optional)

To enable email notifications, add SMTP settings to `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.yourprovider.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@yourdomain.com",
    "SenderPassword": "yourpassword",
    "UseSsl": true
  }
}
```

Then update the `SendQuoteEmail()` method in `HomeController.cs` to use SmtpClient.

## 🚀 Deployment

### Deploy to Azure

1. Right-click the project in Visual Studio
2. Select "Publish"
3. Choose "Azure" as target
4. Follow the wizard to configure App Service

### Deploy to IIS

1. Publish the project:
```bash
   dotnet publish -c Release
```
2. Copy the published files to your IIS web directory
3. Configure IIS application pool to use .NET Core

## 🐛 Known Issues

- Email functionality requires SMTP configuration
- Quote requests are currently saved to text file only
- No database integration (for now)

## 📝 To-Do / Future Enhancements

- [ ] Database integration for quote requests
- [ ] Admin panel for managing quotes
- [ ] Email notification system
- [ ] File upload for CAD drawings
- [ ] Gallery/portfolio section
- [ ] Customer testimonials
- [ ] Blog/news section
- [ ] Multi-language support

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Developer

Developed for Palm Engineering, Gqeberha, South Africa

## 🙏 Acknowledgments

- Font: [Inter by Rasmus Andersson](https://fonts.google.com/specimen/Inter)
- Icons: Unicode emoji characters
- Design inspiration: Modern industrial websites

## 📞 Support

For support, email palmeng@netsurfers.co.za or call 082 921 7644.

---

**Built with ❤️ using ASP.NET Core MVC**
