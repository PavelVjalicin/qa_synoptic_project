using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using FirstCateringLtd.BackService.Models;

namespace FirstCateringLtd.BackService.Data
{
    //Main database context that outlines the interface for any kind of database that one might use.
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

		public DatabaseContext(){}

		public DbSet<Employee> Employees { get; set; }

        public static void SeedData(IServiceProvider serviceProvider)
        {

            using (var serviceScope = serviceProvider
                .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

                context.Database.EnsureCreated();


                context.SaveChanges();


            }

        }
    }
}