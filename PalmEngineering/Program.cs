using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Net.Http.Headers;
using PalmEngineering.Models;

var builder = WebApplication.CreateBuilder(args);

// Do not advertise the web server.
builder.WebHost.ConfigureKestrel(o => o.AddServerHeader = false);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(
    builder.Configuration.GetSection("Business").Get<BusinessInfo>() ?? new BusinessInfo());

builder.Services.AddHsts(o =>
{
    o.MaxAge = TimeSpan.FromDays(365);
});

// Only trusts proxies on loopback by default. Add KnownProxies/KnownNetworks if you sit behind another proxy.
builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Per-IP rate limit on dynamic pages (static files are served before the limiter).
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

var app = builder.Build();

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
app.UseHttpsRedirection();

// Security headers on every response.
app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["X-Frame-Options"] = "DENY";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=(), usb=(), interest-cohort=()";
    h["Cross-Origin-Opener-Policy"] = "same-origin";
    h["Cross-Origin-Resource-Policy"] = "same-origin";

    var csp =
        "default-src 'self'; " +
        "script-src 'self' 'inline-speculation-rules'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "img-src 'self' data: blob:; " +
        "font-src 'self'; " +
        "connect-src 'self'; " +
        "object-src 'none'; " +
        "base-uri 'self'; " +
        "form-action 'self'; " +
        "frame-ancestors 'none'";

    if (app.Environment.IsDevelopment())
    {
        // Report-only in development so hot reload / browser link keep working.
        h["Content-Security-Policy-Report-Only"] = csp;
    }
    else
    {
        h["Content-Security-Policy"] = csp + "; upgrade-insecure-requests";
    }

    // Stale-while-revalidate for plain HTML pages that did not set their own caching.
    ctx.Response.OnStarting(() =>
    {
        var r = ctx.Response;
        if ((HttpMethods.IsGet(ctx.Request.Method) || HttpMethods.IsHead(ctx.Request.Method))
            && r.StatusCode == StatusCodes.Status200OK
            && !r.Headers.ContainsKey(HeaderNames.CacheControl)
            && (r.ContentType?.StartsWith("text/html", StringComparison.OrdinalIgnoreCase) ?? false))
        {
            r.Headers[HeaderNames.CacheControl] = "public, max-age=60, stale-while-revalidate=600";
        }
        return Task.CompletedTask;
    });

    await next();
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Files linked with asp-append-version carry ?v=hash and can be cached forever.
        var versioned = ctx.Context.Request.Query.ContainsKey("v");
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = versioned
            ? "public, max-age=31536000, immutable"
            : "public, max-age=3600, stale-while-revalidate=86400";
    }
});

app.UseRouting();
app.UseRateLimiter();
<<<<<<< Updated upstream

// Static files skip dot-folders, so serve security.txt explicitly.
app.MapGet("/.well-known/security.txt", (BusinessInfo biz) =>
    Results.Text(
        $"Contact: mailto:{biz.Email}\n" +
        $"Expires: {DateTime.UtcNow.AddMonths(11):yyyy-MM-ddTHH:mm:ss.000Z}\n" +
        "Preferred-Languages: en\n",
        "text/plain; charset=utf-8"));
=======
app.UseAuthorization();
>>>>>>> Stashed changes

// Static files skip dot-folders, so serve security.txt explicitly.
app.MapGet("/.well-known/security.txt", (BusinessInfo biz) =>
    Results.Text(
        $"Contact: mailto:{biz.Email}\n" +
        $"Expires: {DateTime.UtcNow.AddMonths(11):yyyy-MM-ddTHH:mm:ss.000Z}\n" +
        "Preferred-Languages: en\n",
        "text/plain; charset=utf-8"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
