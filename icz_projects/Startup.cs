using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using icz_projects.Contexts;
using icz_projects.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace icz_projects
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   options =>
                   {
                       options.LoginPath = new PathString("/login");
                       options.AccessDeniedPath = new PathString("/auth/denied");
                   });

            ProjectContext pctx = new ProjectContext(Configuration.GetSection("XmlDataSources").GetSection("ProjectContext").Value);
            services.AddSingleton(pctx);

            IProjectsRepository irp = new ProjectsRepository(pctx) as IProjectsRepository;
            ILoginRepository ilp = new LoginRepository(Configuration.GetSection("Administration").GetSection("Password").Value, Configuration.GetSection("Administration").GetSection("ClaimName").Value) as ILoginRepository;
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddSingleton(ilp);


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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Projects}/{action=Index}/{id?}");
            });
        }
    }
}
