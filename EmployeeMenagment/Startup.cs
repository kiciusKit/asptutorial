using EmployeeMenagment.Models;
using EmployeeMenagment.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMenagment
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDBContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnectionExpess")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDBContext>()
            .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "1032796905027-lrntn9ht85it6ab2m9hch1v8r8t8p9bm.apps.googleusercontent.com";
                options.ClientSecret = "FiNJK4lntMc2JhCU_7iPzPNH";
                //options.CallbackPath = "";
            });

            services.ConfigureApplicationCookie(options => 
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options => 
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role" , "true")
                //                                                    .RequireRole("Admin")
                //                                                    .RequireRole("Super Admin"));

                //options.AddPolicy("EditRolePolicy",
                //    policy => policy.RequireAssertion(context => 
                //    context.User.IsInRole("Admin") &&
                //    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //    context.User.IsInRole("Super Admin")
                //));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
                options.InvokeHandlersAfterFailure = false;   //This not invoke other requirements handler after failer will called;


            });


            services.AddTransient<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddSingleton<IAuthorizationHandler,CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler,SuperAdminHandler>();


            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 10;
            //    options.Password.RequiredUniqueChars = 3;
            //});
        }

        // This method gets called by the runtime.  Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseStaticFiles();

            app.UseAuthentication();
            //Standard Routing
            app.UseMvcWithDefaultRoute();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
