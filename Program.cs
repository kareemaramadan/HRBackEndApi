
using Azure.Core;
using HR.Application.Interfaces;
using HR.Application.Mapping.LookUpsMapping;
using HR.Application.Services;
using HR.Domain.Models.Identity;
using HR.Infrastructure.Context;
using HR.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRBackEndApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region AddDataBaseConnection

            //Add Identity DataBase Connection
            //=================================
            builder.Services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            //Add Application DataBase Connection
            //=================================
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
            //=========================================================================

            #endregion

            #region AddIdentityServices

            //Add Identity Services
            //=========================

            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<HR.Infrastructure.Context.IdentityContext>()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();

            #endregion


            builder.Services.AddControllers();

            #region SwaggerConfigurationService

            //configure swagger for API documentation
            //========================================
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HRMS Web API",
                    Version = "v1",
                    Description = "This is a HRMS Web API for managing human resource system",
                    Contact = new OpenApiContact
                    {
                        Name = "Kareem Sayed Ramadan",
                        Email = "kramadan@petroamir.com",
                    }
                });
            });

            #endregion


            #region CORSConfigurationService


            // Configure CORS to allow requests from any origin
            //=========================================================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });


            #endregion

            #region RegisterRepositoriesServices

            //Register Repositoriesand Interfaces Services
            //=============================================

            //AddTransient for IBaseRepository and BaseRepository==> This means that a new instance of the repository will be created each time it is requested.
            //This is useful for lightweight, stateless services that do not maintain any state between requests.

            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //AddScoped =>A single object is made for the duration of an entire request (e.g., an HTTP web request).
            //If two classes ask for the service in the same HTTP request, they share the exact same instance.
            //A new instance is only created when a new HTTP request begins.

            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            #endregion

            #region RegisterMappers

            builder.Services.AddAutoMapper(m => { }, typeof(CityMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(CountryMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(CompanyMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(GovernorateMappingProfile));

            #endregion


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region AddMiddleWarePipelines


            // Apply the CORS policy
            //======================
            app.UseCors("CorsPolicy");

            // Enable HTTPS redirection
            //=========================
            app.UseHttpsRedirection();
            app.UseRouting();

            // Enable Authentication and Authorization Middleware
            //===================================================
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #endregion


            app.Run();
        }
    }
}
