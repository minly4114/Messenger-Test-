using System;
using Messenger.Data;
using Messenger.Data.IProviders;
using Messenger.Data.Providers;
using Messenger.EmailAdapters;
using Messenger.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger
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
            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<PostgresContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<DialogsContext>(options => options.UseNpgsql(connection));

            services.AddDefaultIdentity<IdentityUser>(
                options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PostgresContext>();
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddHttpClient();

            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("AuthMessageSenderOptions"));
            services.AddTransient<IEmailSender, EmailSender>()
                .AddScoped<IUserProvider,UserProvider>()
                .AddScoped<IDialogProvider, DialogProvider>()
                .AddSingleton<ILog4netProvider, Log4netProvider>(); 

            services.AddScoped(typeof(IDbSetProvider<>), typeof(DbSetProvider<>));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PostgresContext context, DialogsContext dialogsContext, ILog4netProvider log4Net, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                log4Net.Error(typeof(Startup).ToString(), ex.Message, ex.StackTrace);
            }
            try
            {
                dialogsContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                log4Net.Error(typeof(Startup).ToString(), ex.Message, ex.StackTrace);
            }
        }
    }
}
