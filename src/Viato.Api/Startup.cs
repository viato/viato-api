using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Viato.Api.Entities;

namespace Viato.Api
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
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddDbContext<ViatoContext>(options =>
            {
                options.UseNpgsql(appSettings.PostgresConnectionString);
            });

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ViatoContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(IdentityConfigs.Ids)
            .AddInMemoryApiResources(IdentityConfigs.Apis)
            .AddInMemoryClients(IdentityConfigs.Clients)
            .AddAspNetIdentity<AppUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:5000/";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api";
                });
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = appSettings.GoogleClientId;
            //    googleOptions.ClientSecret = appSettings.GoogleClientSecret;
            //})
            //.AddTwitter(twitterOptions =>
            //{
            //    twitterOptions.ConsumerKey = appSettings.TwitterConsumerKey;
            //    twitterOptions.ConsumerSecret = appSettings.TwitterConsumerSecret;
            //})
            //.AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = appSettings.FacebookAppId;
            //    facebookOptions.AppSecret = appSettings.FacebookAppSecret;
            //});

            services.AddAuthorization(authorizationOptions =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                        JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                authorizationOptions.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Viato API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Viato API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
