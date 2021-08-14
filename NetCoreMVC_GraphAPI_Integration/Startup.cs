using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreMVC_GraphAPI_Integration.Models;
using NetCoreMVC_GraphAPI_Integration.Services;

namespace NetCoreMVC_GraphAPI_Integration
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Credentials>(Configuration.GetSection("Credentials"));
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "127.0.01:6379";
                option.InstanceName = "master";
            });

            services.AddScoped<IRedisCacheService, RedisCacheService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddOptions();
            services.AddControllersWithViews();
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
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
