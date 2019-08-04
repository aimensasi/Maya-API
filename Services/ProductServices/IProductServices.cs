using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Maya.Models;

namespace Maya.Services.ProductServices {
	public interface IProductServices {

		ICollection<object> products();

		Task<(bool state, object response)> store(NewProductRequest request);
		Task<(bool state, object response)> update(NewProductRequest request, int id);
		Task<(bool state, object response)> delete(int id);
		Task<(bool state, object response)> uploadImage(HttpRequest request, int id);
		ICollection<object> transformCollection(ICollection<Product> products);
		object transform(Product product);
	}	
}