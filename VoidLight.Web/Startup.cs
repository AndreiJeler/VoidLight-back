using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Hellang.Middleware.ProblemDetails;
using VoidLight.Business.Services;
using VoidLight.Business.Services.Contracts;
using VoidLight.Data;
using VoidLight.Web.Infrastructure.Middlewares;
using VoidLight.Infrastructure.Common.Exceptions;
using VoidLight.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VoidLight.Data.Entities;
using VoidLight.Web.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using VoidLight.Web.Hubs;

namespace VoidLight.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            CurrentEnvironment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                // For Development, CORS is required as the backend and frontend have different URLs
                services.AddCors(o => o.AddDefaultPolicy(builder => builder
                    .WithOrigins(Configuration["WebBackendUrl"], Configuration["WebFrontendUrl"])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                ));
            }

            services.AddControllersWithViews().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddProblemDetails(config =>
            {
                config.Map<ApiExceptionBase>(exception => new ExceptionProblemDetails(exception));
            });

            RegisterAppServices(services);

            services.AddMvc();

            var key = Configuration.GetSection("AppSettings").GetSection("Secret").Value;

            services.AddAuthentication(options =>
            {
                /* options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;*/
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddSteam(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.SaveTokens = true;

                    options.Events.OnTicketReceived = context_ =>
                    {
                        var steamUserAsClaims = context_.Principal;

                        var identityUser = context_.HttpContext.User;

                        return Task.CompletedTask;

                    };
                    options.Events.OnAuthenticated = context_ =>
                    {
                        var steamUserAsClaims = context_.Identity;
                        var nameIdentifier = steamUserAsClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value.Split('/').Last();
                        var name = steamUserAsClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                        context_.HttpContext.User.Claims.Append(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
                        context_.HttpContext.User.Claims.Append(new Claim(ClaimTypes.Name, nameIdentifier));

                        return Task.CompletedTask;
                    };
                    options.ApplicationKey = Configuration.GetSection("AppSettings").GetSection("SteamKey").Value;
                })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                foreach (var role in Enum.GetValues(typeof(RoleType)))
                {
                    options.AddPolicy(role.ToString(), policy => policy.Requirements.Add(new AccountCustomRequirement((RoleType)role)));
                }
            });

            services.AddTransient<IAuthorizationHandler, AccountCustomRequirementHandler>();
        }

        private void RegisterAppServices(IServiceCollection services)
        {
            // Register DB Context and Config
            services.AddDbContext<VoidLightDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DbConnection")), ServiceLifetime.Scoped);
            services.AddTransient<VoidLightDbConfiguration>();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<FirebaseSettings>(Configuration.GetSection("FirebaseSettings"));


            // Services
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IFriendService, FriendService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGamePublisherService, GamePublisherService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<ISteamClient, SteamClient>();
            services.AddScoped<IDiscordService,  DiscordService>();

            //services.AddSingleton<ISteamGameCollection, SteamGameCollection>();

            /*
             * Register services here
             */

            services.AddSignalR();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbConfiguration = serviceScope.ServiceProvider.GetService<VoidLightDbConfiguration>();
                dbConfiguration.Seed();
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionMiddleware().Invoke
            });

            app.UseCors();

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();


            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<FriendsHub>("/friends-hub");
                endpoints.MapHub<PostsHub>("/posts-hub");
            });
        }
    }
}
