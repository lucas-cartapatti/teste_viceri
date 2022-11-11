using HeroAcademyApi.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace HeroAcademyApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
                setup =>
                {
                    setup.SwaggerDoc(
                            "v1",
                            new Microsoft.OpenApi.Models.OpenApiInfo()
                            {
                                Title = "Hero Academy API",
                                Version = "v1",
                                Description = "Hero Academy API is used for creating new super heroes, updating, checking them and deleting...",
                                Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                                {
                                    Email = "superhero@heroacademy.com",
                                    Name = "superhero"
                                },
                                License = new Microsoft.OpenApi.Models.OpenApiLicense()
                                {
                                    Name = "MIT License"
                                }
                            }
                        );
                }
            );

        var dbVersion = new MariaDbServerVersion(new Version(8, 0, 31));
        services.AddDbContext<heroacademydbContext>(options => options.UseMySql(Configuration.GetConnectionString("Conn"), dbVersion));

        services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        services.AddControllersWithViews().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger(x => x.SerializeAsV2 = true);
        app.UseSwaggerUI(
                setupAction =>
                {
                    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json","Hero Academy Api V1");
                    setupAction.RoutePrefix = String.Empty;
                }
            );

        app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}
