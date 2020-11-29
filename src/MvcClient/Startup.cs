using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcClient.IdentityModel;
using MvcClient.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services.AddScoped<authsampleContext>(); // Add my custom manager
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireClaim("role", "admin"));
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })

            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.Scope.Add("api1");
                options.Scope.Add("profile");
                //options.Scope.Add("offline_access");
                options.SaveTokens = true;
                //options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                //options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");


                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var stream = ctx.TokenEndpointResponse.AccessToken;
                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken(stream);
                        var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                        //var jti = tokenS.Claims;

                        // var claim = new Claim("your-claim", "your-value");

                        // var identity = new ClaimsIdentity(new[] { claim });
                        var identity = new ClaimsIdentity(tokenS.Claims);

                        ctx.Principal.AddIdentity(identity);

                        await Task.CompletedTask;
                    }
                };
            });
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=authsample;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var connectionString = _configuration.GetConnectionString("authdb");
            services.AddDbContextPool<AppUserContext>(o => o.UseSqlServer(connectionString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
           // app.UseIdentity();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
        }
    }
}
