using System;
using System.Threading.Tasks;
using System.Linq;
using Bogus;
using System.Collections.Generic;

using Maya.Models;


namespace Maya.Seeders {
	public class ProductTableSeeder {

		public static async Task run(IServiceProvider servces, BundleContext context) {
			if (context.Products.Any()) {
				return;
			}

			var categories = context.Category.ToList();
			var faker = new Faker("en");
			var products = new List<Product>();

			foreach (var category in categories) {
				for (int i = 0; i < 10; i++) {
					products.Add(new Product {
						Name = faker.Commerce.ProductName(),
						Description = faker.Lorem.Paragraph(),
						Price = Convert.ToDecimal(faker.Commerce.Price()),
						CategoryId = category.Id,
						CreatedAt = DateTimeOffset.Now,
					});
				}
			}
			context.Products.AddRange(products);
			await context.SaveChangesAsync();
		}
	}
}