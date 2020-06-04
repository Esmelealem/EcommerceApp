using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceProduct.Data;
using ECommerceProduct.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceProduct
{
    public class Startup
    {
        //private readonly IHostingEnvironment _env;
       // private IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //if (_env.IsEnvironment("local"))
            //{
            //    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            //}
            //else
            //{
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //}
            services.AddScoped<ApplicationDbContext>();
            services.AddDbContextPool<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("ECommerceDBConnection")));

            //services.AddScoped<IProductRepository,ProductImpl>();
            //services.AddScoped<IHostingStartup, IHostingStartup>();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddDbContext<ApplicationDbContext>(options => options.UseApplicationServiceProvider()));
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                {
                    //used to customize DeveloperExceptionPage
                    SourceCodeLineCount = 1
                };//newly added 
                  //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                  //developerExceptionPageOptions.SourceCodeLineCount = 1;
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                //app.UseDeveloperExceptionPage(); origin
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.Run(async (context) =>
            //    {
            //        // throw new Exception("Error Process is going around");
            //        await context.Response.WriteAsync("Hello Environment: " + env.EnvironmentName);

            //        await context.Response.WriteAsync(System.Diagnostics.
            //            Process.GetCurrentProcess().ProcessName);

            //    });

            //app.UseMvc(routes =>
            //{
            //    //routes.MapRoute("default", "{controller}/{action}/{id}");
            //    routes.MapRoute(
            //       name: "default",
            //       template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area=Customers}/{controller=Home}/{action=Index}/{id?}"
                );
            });

        }

    }
}
