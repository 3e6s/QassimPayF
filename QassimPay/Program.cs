using Microsoft.EntityFrameworkCore;
using QassimPay.Data;
internal class Program
{
    private static void Main(string[] args)
    {
        // Create the builder for configuring services and application setup
        var builder = WebApplication.CreateBuilder(args);

        // Register services for controllers and views
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();


        // Register DbContext with PostgreSQL connection
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add session services
        builder.Services.AddSession();

        // Build the application
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            // Use a custom error page in non-development environments
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // Middleware for HTTPS redirection and serving static files
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Configure request routing
        app.UseRouting();

        // Enable session middleware
        app.UseSession();

        // Add authorization middleware
        app.UseAuthorization();

        // Define a default route for MVC controllers
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=About}/{id?}");

        // Run the application
        app.Run();

    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSession();
        services.AddControllersWithViews();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSession();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }

}
