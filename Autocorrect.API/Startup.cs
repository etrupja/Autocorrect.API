using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autocorrect.API.Data;
using Autocorrect.API.Models;
using Autocorrect.API.Services;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Swashbuckle.AspNetCore.Swagger;

namespace Autocorrect.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Adding Cors Config
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin().
                        AllowAnyMethod().
                        AllowAnyHeader().
                        AllowCredentials());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton(Configuration.GetSection("Licensing").Get<LicenseSettings>());
            var identityServerAuthOptions = Configuration.GetSection("Identity").Get<IdentityServerAuthenticationOptions>();

            //configure services
            services.AddTransient<ILicenseService, LicenseService>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityServerAuthOptions.Authority;
                    options.RequireHttpsMetadata = identityServerAuthOptions.RequireHttpsMetadata;
                    options.ApiName = identityServerAuthOptions.ApiName;
                });

            //Stripe configuration
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Autocorrect API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();

            //Stripe configuration
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Autocorrect API V1");});
            app.UseCors("AllowAll");

            app.UseMvc();
        }
    }
}
