using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ResourceAuth.Models;
using Microsoft.AspNetCore.SpaServices.AngularCli;

namespace ResourceAuth
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var authOptions = Configuration.GetSection("Auth").Get<AuthOptions>();
            var authOptions1 = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptions1);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(Opt =>
                 {
                     Opt.RequireHttpsMetadata = false;
                     Opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidIssuer = authOptions.Issuer,

                         ValidateAudience = true,
                         ValidAudience = authOptions.Audience,

                         ValidateLifetime = true,
                         IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                         ValidateIssuerSigningKey = true,

                     };
                 });

            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(

                    by =>
                    {
                        by.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });

            });
            
            services.AddSingleton(new SlotsStore());
            string connectionString = "Server=tcp:cursov.database.windows.net,1433;Initial Catalog=authdatabase;Persist Security Info=False;User ID=dedmoped;Password=Passw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ng-kurs/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ng-kurs";
                spa.Options.StartupTimeout = new System.TimeSpan(0, 15, 0);
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
          
        }
    }
}
