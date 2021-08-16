using DotnetCoreSampleA.Data;
using DotnetCoreSampleA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Fluent;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace DotnetCoreSampleA
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //X509Certificate2 cert = null;
            //using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            //{
            //    certStore.Open(OpenFlags.ReadOnly);
            //    X509Certificate2Collection certCollection = certStore.Certificates.Find(
            //        X509FindType.FindByThumbprint,
            //        // Replace below with your cert's thumbprint
            //        "8f9cfcf9f6d9370723670efdb8e0da68b6e0c6d6",
            //        false);
            //    // Get the first cert with the thumbprint
            //    if (certCollection.Count > 0)
            //    {
            //        cert = certCollection[0];
            //        Log.Info($"Successfully loaded cert from registry: {cert.Thumbprint}");
            //        //Log.Logger.Information($);
            //    }
            //}

            //// Fallback to local file for development
            //if (cert == null)
            //{
            //    cert = new X509Certificate2(Path.Combine("C:\\Users\\Tayyab\\dotnetcore\\dotnetsamplea.pfx"), "123");
            //    Log.Info($"Falling back to cert from file. Successfully loaded: {cert.Thumbprint}");
            //    //Log.Logger.Information($"Falling back to cert from file. Successfully loaded: {cert.Thumbprint}");
            //}
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();//.AddSigningCredential(cert);

            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}