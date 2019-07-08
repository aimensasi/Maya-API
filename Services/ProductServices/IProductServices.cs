using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Maya.Services.ProductServices {
	public interface IProductServices {

		ICollection<object> products();

		Task<(bool state, object response)> store(NewProductRequest request);
		Task<(bool state, object response)> update(NewProductRequest request, int id);

		Task<(bool state, object response)> delete(int id);
	}
}