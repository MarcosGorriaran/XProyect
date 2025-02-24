using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller.DAO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProyectXAPILibrary.Controller
{
    public class ProfileController : Controller, ICreateAsync<Profile>, IUpdateAsync<Profile>, IDeleteAsync<Profile>
    {
        const string AddProfilePath = "AddProfile";
        const string UpdateProfilePath = "UpdateProfile";
        const string DeleteProfile = "DeleteProfile";
        public ProfileController(HttpClient client) : base(client) { }

        public async Task<ResponseDTO<object>> CreateAsync(Profile profile)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,AddProfilePath);
            HttpResponseMessage response = await SendRequest(request, profile);

            return DeserializeResponse<object>(response);
        }
        public async Task<ResponseDTO<object>> UpdateAsync(Profile profile)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, UpdateProfilePath);
            HttpResponseMessage response = await SendRequest(request, profile);

            return DeserializeResponse<object>(response);
        }
        public async Task<ResponseDTO<object>> DeleteAsync(Profile profile)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, DeleteProfile);
            HttpResponseMessage response = await SendRequest(request, profile);

            return DeserializeResponse<object>(response);
        }
    }
}
