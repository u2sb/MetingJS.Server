using MetingJS.Server.Models;
using MetingJS.Server.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetingJS.Server
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
			services.AddSingleton<AppSettings>(new AppSettings(Configuration));
			services.AddSingleton<IMeting, Meting>();
			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppSettings appSettings)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
				builder.WithOrigins(appSettings.WithOrigins).SetIsOriginAllowedToAllowWildcardSubdomains().WithMethods("GET", "OPTIONS")
					.AllowAnyHeader());

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
