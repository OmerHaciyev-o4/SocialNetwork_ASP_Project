using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Business.Concrete;
using SocialNetwork.DataAccess.Abstract;
using SocialNetwork.DataAccess.Concrete;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Controllers;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Hubs;

namespace SocialNetwork.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IFriendService, FriendManager>();
            services.AddScoped<INotificationService, NotificationManager>();
            services.AddScoped<IPostService, PostManager>();
            services.AddScoped<IPostImageService, PostImageManager>();
            services.AddScoped<IMessageService, MessageCloudManager>();
            services.AddScoped<IRoomService, RoomManager>();

            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IFriendDal, EfFriendDal>();
            services.AddScoped<INotificationDal, EFNotificationDal>();
            services.AddScoped<IPostDal, EfPostDal>();
            services.AddScoped<IPostImageDal, EfPostImageDal>();
            services.AddScoped<IMessageCloudDal, EfMessageCloudDal>();
            services.AddScoped<IRoomDal, EfRoomDal>();

            services.AddSignalR();

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<CustomIdentityDbContext>(options =>
                options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SocialDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

            services.AddIdentity<CustomIdentityUser, CustomIdentityRole>()
                .AddEntityFrameworkStores<CustomIdentityDbContext>()
                .AddDefaultTokenProviders();


            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "681264779056-fif3t8vukijvquajgq80du3q9pl9ocq9.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                })
                .AddFacebook(options =>
                {
                    options.AppId = "332017705584364";
                    options.AppSecret = "73f341163c4c1a681b6b4677a76660f7";
                });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                HomeController.Context = context;
                DatabaseController.Context = context;
                ChatHub.Context = context;

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=home}/{action=index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}