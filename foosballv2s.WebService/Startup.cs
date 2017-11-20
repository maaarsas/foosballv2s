using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace foosballv2s.WebService
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
            services.AddSingleton(Configuration);
            
            services.AddScoped(typeof(IGameRepository), typeof(GameRepository));
            services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddDbContext<WebServiceDbContext>(opt => opt.UseSqlServer(
                //Configuration.GetConnectionString("DbConnectionString")
                "Server=mssql1.gear.host;Database=foosballv2s;User ID=foosballv2s;Password=Ew7f-_w63PCx;"
            ));

            services.AddDbContext<IdentityContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("SecurityConnection"), sqlOptions
                options.UseSqlServer("Server=mssql1.gear.host;Database=foosballv2s;User ID=foosballv2s;Password=Ew7f-_w63PCx;"));
            
            
            services.AddIdentity<User, UserRole>(cfg =>
            {
               
//                cfg.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
//                {
//                    OnRedirectToLogin = context =>
//                    {
//                        if (context.Request.Path.StartsWithSegments("/api"))
//                        {
//                            context.Response.StatusCode = (int) System.Net.HttpStatusCode.Unauthorized;
//                        }
//                        return Task.FromResult(0);
//                    }
//                };
                // configure identity options
                cfg.Password.RequireDigit = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders(); 
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(j =>
                {
//                    j.AutomaticAuthenticate = true;
//                    j.AutomaticChallenge = true;
                    j.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JwtSecurityToken:Issuer"],
                        ValidAudience = Configuration["JwtSecurityToken:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityToken:Key"])),
                        ValidateLifetime = true
                    };
            });
            
            services.AddScoped<IWebServiceDbContext>(provider => provider.GetService<WebServiceDbContext>());
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseIdentity();
        }
    }
}