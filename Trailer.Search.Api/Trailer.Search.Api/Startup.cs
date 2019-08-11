using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMDbLib.Client;
using Trailer.Search.Data.Services;
using Trailer.Search.Data.Services.SearchEngine;
using Trailer.Search.Data.Services.Tmdb;
using Trailer.Search.Data.Services.Youtube;
using Trailer.Search.Infrastructure;

namespace Trailer.Search.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            ConfigureDIForServices(services);
            ConfigureDIForExternalServices(services);
        }

        private static void ConfigureDIForServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ISearchService), typeof(AgregatedSearchService));
            services.AddScoped(typeof(IVideoServiceSearch), typeof(YoutubeSearch));
            services.AddScoped(typeof(IMovieDatabaseSearch), typeof(TmdbSearch));
        }

        private void ConfigureDIForExternalServices(IServiceCollection services)
        {
            services.AddScoped(x => new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Configuration.GetSection("AppSettings")["YoutubeApiKey"],
                ApplicationName = this.GetType().ToString()
            }));

            services.AddScoped(x => new TMDbClient(Configuration.GetSection("AppSettings")["TmdbApiKey"]));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
