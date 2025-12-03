using System.Reflection;
using TIWIKOM.Entities;
using TIWIKOM.Entities.Contexts;
using TIWIKOM.WebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace TIWIKOM.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            var mvcBuilder = builder.Services.AddControllersWithViews();
            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            // Add Razor Pages support for Identity UI
            builder.Services.AddRazorPages();

            // Add logging with Serilog
            builder.Host.UseSerilog();
            ConfigureLogging(builder);

            // Configure Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure Identity
            builder.Services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<TipDataInitializer>();
            builder.Services.AddScoped<RoleDataInitializer>();
            builder.Services.AddScoped<AdminUserInitializer>();
            builder.Services.AddScoped<TipService>();
            builder.Services.AddScoped<InteractionService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                    Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            var app = builder.Build();

            app.UseForwardedHeaders();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Map Razor Pages for Identity UI
            app.MapRazorPages();

            // Initialize database and seed data asynchronously without blocking startup
            _ = InitializeDatabaseAsync(app.Services);

            app.Run();
        }

        private static async Task InitializeDatabaseAsync(IServiceProvider services)
        {
            try
            {
                using (var scope = services.CreateAsyncScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    // Apply pending migrations
                    await context.Database.MigrateAsync();

                    // Initialize roles
                    var roleInitializer = scope.ServiceProvider.GetRequiredService<RoleDataInitializer>();
                    await roleInitializer.InitializeAsync();

                    // Initialize admin user
                    var adminInitializer = scope.ServiceProvider.GetRequiredService<AdminUserInitializer>();
                    await adminInitializer.InitializeAsync();

                    // Initialize tips
                    var tipInitializer = scope.ServiceProvider.GetRequiredService<TipDataInitializer>();
                    await tipInitializer.InitializeAsync();
                    
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Database initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while initializing the database.");
            }
        }
        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var productName = builder.Configuration["AppSettings:ProductName"] ?? "TIWIKOM";
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.WithProperty("Environment", env)
                .Enrich.WithProperty("ApplicationName", Assembly.GetExecutingAssembly().GetName().Name)
                .Enrich.WithProperty("ProductName", productName)
                .Enrich.FromLogContext();

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
