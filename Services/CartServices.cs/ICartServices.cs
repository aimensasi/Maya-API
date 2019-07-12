using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Maya.Services.CartServices {
	public interface ICartServices {

		Task<(bool state, object response)> cart();
	}
}