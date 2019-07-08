using System;
using System.Threading.Tasks;
using System.Linq;

using Maya.Models;




namespace Maya.Seeders {
	public static class CategoryTableSeeder {

		public static async Task run(IServiceProvider servces, BundleContext context) {
			if (context.Category.Any()) {
				return;
			}

			var categories = new[] { "Bracelet", "Wedding Band", "Ring", "Necklace", "Earring" };

			foreach (var category in categories) {
				context.Category.Add(
					new Category{ Name = category}
				);
			}

			await context.SaveChangesAsync();
		}
	}
}