using Maya.RequestProperties;
using System.Threading.Tasks;

namespace Maya.Services.UserServices {
	public interface IUserServices {

		bool IsAdminRegisterOpen();
		Task<(bool state, object response)> create(RegisterRequest request, string role = null);
	}
}