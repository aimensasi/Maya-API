using Maya.RequestProperties;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;
using Maya.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Configuration;


namespace Maya.Services.ProductServices {
	public class ProductServices : IProductServices {

		private readonly string FILE_PATH = "storage/products";
		private readonly BundleContext _context;
		private readonly IHostingEnvironment _env;
		private readonly IHttpContextAccessor _httpContext;
		private readonly IConfiguration _configuration;
		private readonly string _access_key;

		public ProductServices(BundleContext context, IHostingEnvironment env, IHttpContextAccessor httpContext, IConfiguration configuration) {
			_context = context;
			_env = env;
			_httpContext = httpContext;
			_configuration = configuration;

			_access_key = _configuration.GetConnectionString("BlobAccessKey");
		}

		public ICollection<object> products() {
			var products = _context.Products.ToList();

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
			var product = await _context.Products.Include(p => p.Category).SingleOrDefaultAsync(p => p.Id == id);

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

		public async Task<(bool state, object response)> delete(int id) {
			var product = await _context.Products.FindAsync(id);

			if (product == null) {
				return (false, new { message = "Product Not Found or Does not exists" });
			}

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return (true, new { message = "Product deleted successfully" });
		}

		public async Task<(bool state, object response)> uploadImage(HttpRequest Request, int id) {

			IFormFile image = Request.Form.Files[0];

			if (image.Length == 0) {
				return (false, new { message = "Provided media is not valid" });
			}

			var product = await _context.Products.FindAsync(id);

			if (product == null) {
				return (false, new { message = "Product Not Found or Does not exists" });
			}

			try {

				string filename = generateFileName(image);

				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this._access_key);

				CloudBlobClient client = storageAccount.CreateCloudBlobClient();

				CloudBlobContainer container = client.GetContainerReference("products");

				CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);



				using (var fileStream = image.OpenReadStream()) {
					await blockBlob.UploadFromStreamAsync(fileStream);
				}

				ProductImage productImage = new ProductImage {
					ProductId = product.Id,
					Image = blockBlob.Uri.AbsoluteUri,
				};

				await _context.ProductImages.AddAsync(productImage);
				await _context.SaveChangesAsync();

				return (true, new { message = "Upload Completed", url = productImage.Image });

			} catch (Exception e) {
				return (false, new { message = "Something Went wrong, please try again later" });
			}
		}



		/* Product Transformation */

		private void createDirIfNotExists(string path) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		private string generateFileName(IFormFile file) {
			string ext = Path.GetExtension(file.FileName);
			string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

			using (MD5 md5 = MD5.Create()) {
				byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(filename);
				byte[] hashBytes = md5.ComputeHash(fileBytes);

				// Convert the byte array to hexadecimal string
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++) {
					sb.Append(hashBytes[i].ToString("X2"));
				}
				filename = sb.ToString() + ext;

				return filename;
			}
		}

		private string transformUrl(string filename) {
			return filename;
		}

		private List<object> transformProductImages(Product product) {
			var productImages = new List<object>();

			foreach (ProductImage productImage in product.ProductImages) {
				productImages.Add(transformUrl(productImage.Image));
			}

			return productImages;
		}

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
				images = transformProductImages(product),
			};
		}
	}
}