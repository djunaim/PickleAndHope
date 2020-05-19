using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PickleAndHope.DataAccess;

namespace PickleAndHope
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
            services.AddControllers();

            //service registration - tell asp.net core this thing that does particular job
            //container - holds registration
            //all 3 below are defining lifecyles of class

            //transient class shortest lifecycle. Class gets created every time someone asks. Gives new copy every time even within same api request and will go away once no longer used
            //if have more than 1 reposiory, need to tell asp.net core about the different repos to make
            services.AddTransient<PickleRepository>();

            //give new copy, but only new one for every api request
            //services.AddScoped<>();

            //while app running, only create one copy and share it
            //tell api.net core when someone asks for IConfiguration, give what's in parentheses, which is Configuration
            //if have more than 1 configuration, need to tell asp.net core about the different configs to make
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // how to do asp.net side of things
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
