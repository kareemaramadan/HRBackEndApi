using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Application.Mapping.AuthMapping;
using HR.Application.Mapping.LookUpsMapping;
using HR.Application.Services;
using HR.Domain.Models.Identity;
using HR.Infrastructure.Context;
using HR.Infrastructure.Repository;
using HRBackEndApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace HRBackEndApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region AddDataBaseConnection

            //Add Identity DataBase Connection
            //=================================
            // Configure a separate MigrationsHistoryTable (and migrations assembly) so Identity migrations
            // are stored separately from the application DbContext migrations and do not conflict.
            builder.Services.AddDbContext<IdentityContext>(options =>
            ((builder.Environment.IsDevelopment()) ? options.EnableSensitiveDataLogging(true) : options.EnableSensitiveDataLogging(false))
                   .UseSqlServer(
                    builder.Configuration.GetConnectionString("IdentityConnection"),
                    sqlOptions => sqlOptions
                        .MigrationsAssembly(typeof(IdentityContext).Assembly.FullName))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            //.MigrationsHistoryTable("__Identity_MigrationsHistory", "Auth")

            //Add Application DataBase Connection
            //=================================
            // Configure a distinct MigrationsHistoryTable for application DbContext to avoid mixing
            // migrations with the Identity context.Optionally place App migrations in the default schema.
            //builder.Services.AddDbContext<AppDbContext> ( options =>
            //    options.UseSqlServer (
            //        builder.Configuration.GetConnectionString ( "HRDbConnection" ),
            //        sqlOptions => sqlOptions
            //            .MigrationsAssembly ( typeof ( AppDbContext ).Assembly.FullName ) ) );
            //.MigrationsHistoryTable ( "__App_MigrationsHistory", "dbo" )
            //=========================================================================

            #endregion


            #region AddIdentityServices

            //Add Identity Services
            //=========================

            builder.Services.AddIdentity<AppUser, AppRole>(
                options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                    options.User.RequireUniqueEmail = true;

                    // Lockout Settings
                    // Lockout new users
                    options.Lockout.AllowedForNewUsers = builder.Configuration.GetValue<bool>("Identity:Lockout:AllowedForNewUsers", true);
                    // Number of failed attempts allowed
                    options.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>("Identity:Lockout:MaxFailedAccessAttempts", 5);
                    // Lockout duration
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
                        builder.Configuration.GetValue<int>("Identity:Lockout:DefaultLockoutMinutes", 5));
                })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();





            #endregion

            #region Controllers & Localization
            //=================================

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            }
           );

            var supportedCultures = new[]
            {
    new CultureInfo("en-US"),
    new CultureInfo("ar-EG")
   };
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };
            #endregion

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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                // 
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token."
                });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });

            });

            #endregion


            #region CORSConfigurationService


            // Configure CORS to allow requests from any origin
            //=========================================================
            // ----- CORS (restrict origins) -----
            // Provide allowed origins via configuration "Cors:AllowedOrigins" as semicolon-separated values.
            // Example appsettings: "Cors": { "AllowedOrigins": "https://app.example.com;https://admin.example.com" }
            var allowedOriginsConfig = builder.Configuration.GetValue<string>("Cors:AllowedOrigins") ?? string.Empty;
            var allowedOrigins = allowedOriginsConfig.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(s => s.Trim())
                                                     .ToArray();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });
            });


            #endregion

            #region RegisterServices

            //Register Repositoriesand Interfaces Services
            //=============================================

            //AddTransient for IBaseRepository and BaseRepository==> This means that a new instance of the repository will be created each time it is requested.
            //This is useful for lightweight, stateless services that do not maintain any state between requests.

            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //AddScoped =>A single object is made for the duration of an entire request (e.g., an HTTP web request).
            //If two classes ask for the service in the same HTTP request, they share the exact same instance.
            //A new instance is only created when a new HTTP request begins.

            builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            // Register Localization service for Translation Layer between Languages
            builder.Services.AddScoped<ILocalizationService, LocalizationService>();





            #endregion

            #region Register Jwt Authentication Service

            // Register JwtOptions from configuration
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
            // If JwtOptions is required as a direct injectable type (not IOptions), register as singleton
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtOptions>>().Value);
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.RequireHttpsMetadata = true;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Jwt");
                        logger.LogInformation($"Jwt authentication Information: {ctx.Result?.ToString()}");
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Jwt");
                        logger.LogWarning("Jwt authentication failed: {Error}", ctx.Exception?.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Jwt");
                        logger.LogDebug("Jwt token validated for subject {sub}", ctx.Principal?.Identity?.Name ?? "<unknown>");
                        return Task.CompletedTask;
                    },
                    OnChallenge = ctx =>
                    {
                        var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Jwt");
                        logger.LogWarning("Jwt challenge: {Error} {Description}", ctx.Error, ctx.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion

            #region RegisterMappers

            builder.Services.AddAutoMapper(m => { }, typeof(CityMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(CountryMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(CompanyMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(GovernorateMappingProfile));
            builder.Services.AddAutoMapper(m => { }, typeof(AuthMappingProfile));

            #endregion

            var app = builder.Build();

            #region Production-only hardening
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            #endregion

            #region AddMiddleWarePipelines
            // Apply the CORS policy
            //======================
            app.UseCors("CorsPolicy");

            app.UseRequestLocalization(localizationOptions);
            // Enable HTTPS redirection
            //=========================
            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<ProfilingMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

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
