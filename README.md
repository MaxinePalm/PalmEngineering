# Palm Engineering - ASP.NET Core MVC Website

## Project Overview

ASP.NET Core MVC web application for Palm Engineering, a precision CNC machining company.

![Palm Engineering](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue)
![License](https://img.shields.io/badge/license-MIT-green)

## Tech Stack

- ASP.NET Core 6.0+
- C# MVC Pattern
- Razor Views
- CSS3 with CSS Variables

## Project Structure
```
PalmEngineering/
├── Program.cs                      # Application entry point
├── Controllers/
│   └── HomeController.cs           # Handles all page routing and form submission
├── Models/
│   ├── Service.cs                  # Service entity
│   ├── QuoteRequest.cs             # Quote form model with validation
│   └── ErrorViewModel.cs           # Error handling
└── Views/
    ├── _ViewImports.cshtml         # Global imports
    ├── _ViewStart.cshtml           # Default layout config
    ├── Home/
    │   ├── Index.cshtml            # Homepage
    │   ├── About.cshtml            # About page
    │   ├── Services.cshtml         # Services listing
    │   └── Contact.cshtml          # Contact form
    └── Shared/
        ├── _Layout.cshtml          # Master layout with all CSS
        └── Error.cshtml            # Error page
```

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

## Key Features

### Controllers
- `HomeController.cs` - Main controller with Index, About, Services, Contact actions

### Models
- `Service` - Properties: Id, Name, Description, IconClass
- `ErrorViewModel` - Standard error handling

### Views
- `_Layout.cshtml` - Contains all CSS (full-width responsive design)
- Form validation using ASP.NET Core tag helpers
- Success/error messages via TempData

## CSS Architecture

All styles in `_Layout.cshtml` using CSS custom properties:
```css
:root {
    --primary: #0f2557;
    --secondary: #2563eb;
    --accent: #3b82f6;
}
```

- Responsive grid layouts
- Full-width design (100% viewport)
- Sticky header navigation
- Hover effects and transitions

## Form Handling

Quote requests flow:
1. User submits form → `[HttpPost] Contact(QuoteRequest request)`
2. Model validation via DataAnnotations
3. Save to `quote_requests.txt` via `SendQuoteEmail()`
4. Success message via `TempData`
5. Redirect to prevent resubmission

## Namespaces

All files use `PalmEngineering` namespace:
- `PalmEngineering.Controllers`
- `PalmEngineering.Models`

## Configuration

No external configuration required. Quote requests saved to local text file.

Optional: Add SMTP settings to `appsettings.json` for email functionality.

## Dependencies

No additional NuGet packages required beyond default ASP.NET Core MVC template.


## Configuration

Legal and contact details live in `appsettings.json` under `Business` (VAT number, directors, Information Officer).
Empty values are hidden on the site. Do not commit secrets: use `dotnet user-secrets` or environment variables.

## Security notes

Security headers and a CSP are set in `Program.cs`, per-IP rate limiting is enabled, and the site sets no cookies.
No third-party scripts or fonts are loaded (Inter is self-hosted).
