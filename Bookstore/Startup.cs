using Bookstore.Persistence.Models;
using Bookstore.Persistence.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore
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
            DbType dbType = Configuration.GetValue<DbType>("DbType");

            switch (dbType)
            {
                case DbType.SqlServer:
                    // Need Microsoft.EntityFrameworkCore.SqlServer package for this
                    services.AddDbContext<BookStoreDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"));
                        options.UseLazyLoadingProxies();
                    });
                    break;

                case DbType.Sqlite:
                    // Need Microsoft.EntityFrameworkCore.Sqlite package for this
                    services.AddDbContext<BookStoreDbContext>(options =>
                    {
                        options.UseSqlite(Configuration.GetConnectionString("SqliteServerConnection"));
                        options.UseLazyLoadingProxies();
                    });
                    break;

            }
            services.AddIdentity<BaseUser, IdentityRole<int>>(options =>
             {
                 options.Password.RequireDigit = false;
                 options.Password.RequiredLength = 3;
                 options.Password.RequiredUniqueChars = 0;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireUppercase = false;
             })
            .AddEntityFrameworkStores<BookStoreDbContext>()
            .AddDefaultTokenProviders();

            services.AddTransient<IBookStoreService, BookStoreService>();

            services.AddHttpContextAccessor();//Delete

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Books}/{action=Index}/{id?}");
            });
            //BookStoreDbContext context = services.GetRequiredService<BookStoreDbContext>();
            String imageDirectory = Configuration["ImageStore"];
            DbInitializer.Initialize(services,imageDirectory);
        }
    }
}
