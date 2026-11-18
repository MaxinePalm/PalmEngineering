# Palm Engineering

ASP.NET Core 9 MVC website for Palm Engineering, a precision CNC machining workshop in Gqeberha.
It is an information site: no database, no forms, no accounts, no cookies.

## Run

```bash
dotnet run
```

Then open `http://localhost:5206` (or `https://localhost:7284`).

## Structure

```
PalmEngineering.slnx         Solution
PalmEngineering.csproj       Project
Program.cs                   Pipeline: security headers + CSP, HSTS, rate limiting, static file caching
appsettings.json             Business details (VAT number, directors, Information Officer)
Controllers/                 HomeController: Index, About, Services, Gallery, Contact, Privacy, Cookies, Terms, Error
Models/                      BusinessInfo, Service, GalleryItem, GalleryCatalog, Icons, ErrorViewModel
Views/
  Shared/                    _Layout (header, footer, command menu) and Error
  Home/                      One view per page
Properties/                  launchSettings.json
wwwroot/
  css/site.css               All styles (design tokens at the top)
  js/site.js                 Command menu, gallery lightbox, shortlist, toasts, shortcuts
  js/blurhash.js             Image placeholders
  fonts/                     Self-hosted Inter and its licence
  images/gallery/            Project photos
```

## Design

- Colours and spacing are tokens at the top of `site.css` (navy and blue, Inter, white cards on soft grey sections).
- Layout is fluid: grids use `auto-fit` and the header wraps instead of overflowing.

## Configuration

Legal and contact details live in `appsettings.json` under `Business` (VAT number, directors, Information Officer).
Empty values are hidden on the site. Do not commit secrets: use `dotnet user-secrets` or environment variables.

## Security notes

Security headers and a CSP are set in `Program.cs`, per-IP rate limiting is enabled, and the site sets no cookies.
No third-party scripts or fonts are loaded (Inter is self-hosted). The only data kept in the browser is the
gallery shortlist and the cookie-notice choice (see the Privacy Policy and Cookie Policy).
