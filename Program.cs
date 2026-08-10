
using HR.Domain.Models.Identity;
using HR.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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
            builder.Services.AddDbContext<HR.Infrastructure.Context.IdentityContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

            //Add Application DataBase Connection
            //=================================
            builder.Services.AddDbContext<HR.Infrastructure.Context.AppDbContext>(options =>
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

            //builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            #endregion

            #region RegisterMappers

            //builder.Services.AddAutoMapper(m => { }, typeof(CityMappingProfile));
            //builder.Services.AddAutoMapper(m => { }, typeof(CountryMappingProfile));
            //builder.Services.AddAutoMapper(m => { }, typeof(CompanyMappingProfile));
            //builder.Services.AddAutoMapper(m => { }, typeof(GovernorateMappingProfile));

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
