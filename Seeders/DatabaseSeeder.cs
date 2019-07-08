using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using Maya.Models;



namespace Maya.Seeders {
	public static class DatabaseSeeder {

		public static async Task Seed(IServiceProvider services){
			var context = services.GetRequiredService<BundleContext>();

			
			await CategoryTableSeeder.run(services, context);
			await ProductTableSeeder.run(services, context);
		}
	}
}