using MetingJS.Server.Models;
using MetingJS.Server.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static MetingJS.Server.Models.AppSettings;

namespace MetingJS.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			Config = new AppSettings(configuration);
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IMeting, Meting>();
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(builder =>
					builder.WithOrigins(Config.WithOrigins).WithMethods("GET", "OPTIONS")
						.AllowAnyHeader().AllowCredentials());
			}
			else
			{
				app.UseCors(builder =>
					builder.WithOrigins(Config.WithOrigins).WithMethods("GET", "OPTIONS")
						.AllowAnyHeader().AllowCredentials());
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
