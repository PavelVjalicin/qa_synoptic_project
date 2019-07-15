using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FirstCatering.BackService.Models;

namespace FirstCatering.BackService.Data
{
    //Main database context that outlines the interface for any kind of database that one might use.
	public class DataBaseContext : DbContext
	{
		public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options){}

		public DataBaseContext(){}

		public DbSet<Employee> Employees { get; set; }

        public static void SeedData(IServiceProvider serviceProvider)
        {

            using (var serviceScope = serviceProvider
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataBaseContext>();

                context.Database.EnsureCreated();


                context.SaveChanges();


            }

        }
    }
}