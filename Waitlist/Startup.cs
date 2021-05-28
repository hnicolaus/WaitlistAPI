using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;
using Infrastructure.DbContexts;
using Domain.Services;
using Domain.Repositories;
using Infrastructure.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using Domain;

namespace Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
            Configuration = configBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
            });

            //Symmetric Auth: same key is used for both signing & validating bearer tokens.
            IConfigurationSection encryptionSection = Configuration.GetSection("Encryption");
            Encryption encryptionConfig = encryptionSection.Get<Encryption>(); //map appsettings.json to Encryption object
            byte[] symmetricKey = Encoding.UTF8.GetBytes(encryptionConfig.SymmetricKey);
            services.AddAuthentication(symmetricKey);

            services.Configure<Encryption>(encryptionSection); //Allows the Encryption instance to be injected in TokenService's ctor

            services.AddDbContext<WaitlistDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("WaitlistDbContext"), options =>
                {
                    options.UseRelationalNulls(true); //EF Core not compensate https://docs.microsoft.com/en-us/ef/core/querying/null-comparisons
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery); //EF Core default
                }
            ));

            services.AddTransient<WaitlistService>();
            services.AddTransient<CustomerService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<TokenService>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IWaitlistRepository, WaitlistRepository>();
            services.AddScoped<IWaitlistDbContext, WaitlistDbContext>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Waitlist", Version = "v1" });
            });
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Waitlist v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, byte[] symmetricKey)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.SaveToken = true; //saves access token per request to be retrieved in controllers via HttpContext.Request.Headers[HeaderNames.Authorization]
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false, //uncomment line below when set to true
                        //ValidAudiences = new[] { "front_end_client_id_1", "front_end_client_id_2", "..." },
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                        ValidateLifetime = true,
                        LifetimeValidator = LifetimeValidator
                    };
                });

            return services;
        }

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            return expires != null && expires > DateTime.Now;
        }
    }
}
