using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using foosballv2s.WebService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            services.AddScoped(typeof(IGameRepository), typeof(GameRepository));
            services.AddScoped(typeof(ITeamRepository), typeof(TeamRepository));
            services.AddDbContext<WebServiceDbContext>(opt => opt.UseSqlServer(
                //Configuration.GetConnectionString("DbConnectionString")
                "Server=mssql1.gear.host;Database=foosballv2s;User ID=foosballv2s;Password=Ew7f-_w63PCx;"
            ));
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
        }
    }
}