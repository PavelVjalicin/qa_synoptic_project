using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using FirstCateringLtd.BackService.Data;

namespace FirstCateringLtd.BackService
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
			services.AddMvc();

            //Adds a db context option for Sqlite
            services.AddDbContext<DatabaseContext>(options=> options.UseSqlite("Data Source=CateringDatabase.db"));

            // Generates swagger json documentation file
			services.AddSwaggerGen(options =>
					options.SwaggerDoc("v1", new Info { Title = "First Catering Ltd", Version = "v1" })
			);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{

			app.UseSwagger();

			if (env.IsDevelopment() || env.IsStaging())
			{
                // Initialises swagger API Documentation at http://{host}/swagger
                app.UseSwaggerUI(options =>
						options.SwaggerEndpoint("/swagger/v1/swagger.json", "First Catering Ltd v1")
				);

			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

            // Creates built-in database.
			DatabaseContext.SeedData(app.ApplicationServices);

		}
	}
}
