using Maya.RequestProperties;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Maya.Models;
using System.Collections.Generic;
using System;

namespace Maya.Services.ProductServices {
	public class ProductServices : IProductServices {

		private readonly BundleContext _context;

		public ProductServices(BundleContext context) {
			_context = context;
		}

		public ICollection<object> products() {
			var products = _context.Products.Include(product => product.Category).ToList();
			return transformCollection(products);
		}

		public async Task<(bool state, object response)> store(NewProductRequest request) {

			var category = await _context.Category.FindAsync(request.CategoryId);

			if (category == null) {
				return (false, new { message = "Selected Category does not exists" });
			}

			var product = new Product {
				Name = request.Name,
				Description = request.Description,
				Price = request.Price,
				CategoryId = request.CategoryId,
				CreatedAt = DateTimeOffset.Now
			};

			await _context.AddAsync(product);
			await _context.SaveChangesAsync();


			return (true, transform(product));
		}

		public async Task<(bool state, object response)> update(NewProductRequest request, int id) {
			var product = await _context.Products.Include(p => p.Category).SingleOrDefaultAsync( p => p.Id == id);

			if (product == null) {
				return (false, new { message = "Product Not Found or Does not exists" });
			}

			product.Name = request.Name;
			product.Description = request.Description;
			product.Price = request.Price;
			product.CategoryId = request.CategoryId;

			await _context.SaveChangesAsync();

			return (true, transform(product));
		}

		/* Product Transformation */
		public ICollection<object> transformCollection(ICollection<Product> products) {
			var productCollection = new List<object>();

			foreach (Product product in products) {
				productCollection.Add(transform(product));
			}

			return productCollection;
		}

		public object transform(Product product) {
			return new {
				id = product.Id,
				name = product.Name,
				description = product.Description,
				category = product.Category.Name,
				price = product.Price,
				createdAt = product.CreatedAt,
			};
		}
	}
}