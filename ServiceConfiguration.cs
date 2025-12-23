using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Account;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.Repository;
using AdvanceAPI.Services;
using AdvanceAPI.Services.Account;
using AdvanceAPI.Services.DB;
using Serilog;
using Serilog.Core;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using AdvanceAPI.IServices.Approval;
using AdvanceAPI.Services.Inclusive;
using AdvanceAPI.IServices.Inclusive;
using AdvanceAPI.IServices.VenderPriceComp;
using AdvanceAPI.Services.Approval;
using AdvanceAPI.Services.VenderPriceCompServices;
using AdvanceAPI.IServices.Budget;
using AdvanceAPI.Services.Budget;
using AdvanceAPI.IServices.Advance;
using AdvanceAPI.Services.Advance;
using AdvanceAPI.IServices.FirmPaideport;
using AdvanceAPI.Services.FirmPaid;
using AdvanceAPI.IServices.Media;
using AdvanceAPI.Services.Media;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace AdvanceAPI
{

    public class ServiceConfiguration()
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration _config)
        {

            services.AddControllers();
            services.AddOpenApi();

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("https://localhost:4200", "https://localhost:3000")
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .WithHeaders("Content-Type", "Authorization");
                });
            });

            #region API Version - Swagger

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = _config["Jwt:Issuer"],
                     ValidAudience = _config["Jwt:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!))
                 };
             });

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer hgdsfchgdsfghBEARERTOKEN_ghgfsdgfsdgfj\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            #endregion

            // Add Swagger services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<Logger>(new LoggerConfiguration().WriteTo.Seq("http://localhost:5341").CreateLogger());



            var redisConnection = _config.GetValue<string>("Redis:Configuration");
            if (!string.IsNullOrWhiteSpace(redisConnection))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnection;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            //connection string setup
            services.AddSingleton<IDBConnectionStrings, DBConnectionStrings>();

            //DB Connection Setup - SQL
            services.AddScoped<IMySQLConnection, MySQLConnection>();
            services.AddScoped<IDBOperations, DBOperations>();

            //Repository Setup
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IIncusiveRepository, IncusiveRepository>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();
            services.AddScoped<IVenderPriceCompRepository, VenderPriceCompRepository>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IBudgetV2Repository, BudgetV2Repository>();
            services.AddScoped<IAdvanceRepository, AdvanceRepository>();
            services.AddScoped<IFirmPaidRepository, FirmPaidRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();

            //Services Setup
            services.AddScoped<IGeneral, General>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IInclusiveService, InclusiveService>();
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped<IVenderPriceComparisionServices, VenderPriceCompServices>();
            services.AddScoped<IBudget, BudgetService>();
            services.AddScoped<IBudgetV2, BudgetV2Service>();
            services.AddScoped<IAdvanceService, AdvanceService>();
            services.AddScoped<IFirmPaidServices, FirmPaidService>();
            services.AddScoped<IMediaService, MediaService>();
        }
    }

}

