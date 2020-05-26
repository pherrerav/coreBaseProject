using System;
using AutoMapper;
using baseproject.data;
using baseproject.core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.Webpack;
using System.IO;
using baseproject.data.Repositorio.usuario;
using baseproject.data.Repositorio;
using baseproject.core.perfil;
using baseproject.core.usuario;
using proyectomvc.core.pagina;
using baseproject.core.seguridad;
using Microsoft.AspNetCore.Http;
using baseproject.data.Repositorio.perfil;
using baseproject.data.Models;
using Microsoft.Extensions.Logging;
using baseproject.core.logsList;

namespace baseproject
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            var contentRoot = env.ContentRootPath;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           services.AddDbContext<MyDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("connectionString"),
           b => b.MigrationsAssembly("baseproject")));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
            services.AddScoped(typeof(IUsuarioRepositorio), typeof(UsuarioRepositorio));
            services.AddScoped(typeof(IPerfilRepositorio), typeof(PerfilRepositorio));
            services.AddScoped(typeof(IPerfilServicio), typeof(PerfilServicio));
            services.AddScoped(typeof(IPaginaServicio), typeof(PaginaServicio));
            services.AddScoped(typeof(IUsuarioServicio), typeof(UsuarioServicio));
            services.AddScoped(typeof(ILogsListServicio), typeof(LogsListServicio));
            services.Configure<LdapConfig>(Configuration.GetSection("Ldap"));
            services.AddScoped<IAuthenticationService, UserAuthentication>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.Cookie.Name = ".proyecto.Session";
                options.IdleTimeout = TimeSpan.FromHours(8);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromHours(8);
                options.LoginPath = "/Security/Login";
                options.LogoutPath = "/Security/Logout";
                //options.AccessDeniedPath = "/Error/AccesoDenegado";
                options.SlidingExpiration = true;
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSession(); // add session
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("wwwroot/Logs/logError-{Date}.txt");
            DeveloperExceptionPageOptions pageOptions = new DeveloperExceptionPageOptions { SourceCodeLineCount = 10 };
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(pageOptions);
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp"),
                    HotModuleReplacement = true
                });
                //app.UseStatusCodePages();
                //app.UseStatusCodePagesWithRedirects("/Error/StatusCodeHandle");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            else
            {
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Logs}/{action=Index}/{id?}");
                routes.MapRoute(
                   name: "login",
                   template: "/login",
                   defaults: new { controller = "Security", action = "Login" });
                routes.MapRoute(
                   name: "iniciar",
                   template: "/iniciar/{id}",
                   defaults: new { controller = "Security", action = "CheckLogin" });
                routes.MapRoute(
                    name: "logout",
                    template: "/logout",
                    defaults: new { controller = "Security", action = "Logout" });
                routes.MapRoute(
                    name: "usuarios",
                    template: "/usuarios/{id}",
                    defaults: new { controller = "Usuarios", action = "Index" });
                routes.MapRoute(
                   name: "perfiles",
                   template: "/perfiles/{id}",
                   defaults: new { controller = "Perfiles", action = "Index" });
                routes.MapRoute(
                   name: "logs",
                   template: "/logs/{id}",
                   defaults: new { controller = "Logs", action = "Index" });
                routes.MapRoute(
                   name: "error",
                   template: "/error/{statusCode}",
                   defaults: new { controller = "Error", action = "StatusCodeHandle" });
            });
        }
    }
}
