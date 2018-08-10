using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipUploader.Contracts.Services;
using ZipUploader.Data;
using ZipUploader.Data.Abstractions;
using ZipUploader.Domain.Services;
using ZipUploader.Common.Web.Extensions;
using ZipUploader.Common.Web.Formatters;

namespace ZipUploader.API.DataManagementSystem
{
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            const string connStrKey = "DefaultConnection";

            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
            });

            // Database context:
            var connStr = Configuration.GetConnectionString(connStrKey);
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connStr));
            services.AddScoped<IDbContext, ApplicationContext>();

            // Register application services:
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IUploadService, ZipUploadService>();
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseBasicAuthentication();
            app.UseErrorLogging();
            app.UseMvc();

            // Create database and tables using added migrations:
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                context.Database.Migrate();
            }
        }
    }
}
