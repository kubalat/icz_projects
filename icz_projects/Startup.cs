using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using icz_projects.Contexts;
using icz_projects.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

            //Add encoding compatibility
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //Add cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   options =>
                   {
                       options.LoginPath = new PathString("/login");
                   });

            //Set project context
            ProjectContext pctx = new ProjectContext(Configuration.GetSection("XmlDataSources").GetSection("ProjectContext").Value);
            services.AddSingleton(pctx);

            //Set projects repository
            IProjectsRepository irp = new ProjectsRepository(pctx) as IProjectsRepository;
            services.AddScoped<IProjectsRepository, ProjectsRepository>();

            //Set login repository
            ILoginRepository ilp = new LoginRepository(Configuration.GetSection("Administration").GetSection("Password").Value, Configuration.GetSection("Administration").GetSection("ClaimName").Value) as ILoginRepository;
            services.AddSingleton(ilp);

            //Set logger repository
            ILogger logger = new Logger(Configuration.GetSection("LogFilePath").Value) as ILogger;
            services.AddSingleton(logger);
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

            //Enabe authentication
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
