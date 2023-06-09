using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Serialization;
using react_weatherapp.Helpers;
using Microsoft.EntityFrameworkCore;
using react_weatherapp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using react_weatherapp.Controllers;

namespace react_weatherapp
{
    public class Program
    {
      
        public static void Main(string[] args)
        {

            /* EnableCorsAttribute cors = new EnableCorsAttribute(); */
            var builder = WebApplication.CreateBuilder(args);

            // Connection String From appsettings
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddUserSecrets<Program>().Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var storage_connection = configuration.GetValue<string>("StorageConnectionString:connString");
            var apiKey = configuration.GetValue<string>("OpenWeatherApiKey:apiKey");


            builder.Services.Configure<OpenWeatherApiKey>(configuration.GetSection("OpenWeatherApiKey"));
            builder.Services.Configure<StorageKey>(configuration.GetSection("StorageConnectionString"));

            // Dependacy injection for connection 
            builder.Services.AddSingleton<Connection>();
            
            // This is incorporated into connection.cs file, will dig more later to minimize usage
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IWeatherApiService, Controllers.WeatherApiService>();
            builder.Services.AddSingleton<IDownloadApiService, Controllers.DownloadApiService>();
     
            builder.Services.AddHttpClient("PublicWeatherApi", client => client.BaseAddress = new Uri("https://api.openweathermap.org"));

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());




            //JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });




            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<AppDbContext>();
                    /* dbContext.Database.EnsureCreated(); */
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while creating the database
                    Console.WriteLine(ex.Message);
                }
            }

            // Configure the HTTP request pipeline.

             if (app.Environment.IsDevelopment())
                {
                app.UseDeveloperExceptionPage();
                } 

            if (!app.Environment.IsDevelopment())
                {
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //app.MapControllerRoute(
              //  name: "default",
                //pattern: "{controller}/{action=Index}/{id?}");

            //app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
