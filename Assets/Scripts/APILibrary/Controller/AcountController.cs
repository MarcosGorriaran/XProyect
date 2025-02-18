using System.Net.Http.Json;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller.DAO;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProyectXAPILibrary.Controller
{
    public class AcountController : Controller, ICreateAsync<Acount>, IDeleteAsync<Acount>
    {
        const string CheckLoginPath = "CheckLogin";
        const string AddCountPath = "AddAcount";
        const string GetAcountProfilesPath = "GetAcountProfiles";
        const string DeleteAcountPath = "DeleteAcount";
        const string UpdatePasswordPath = "UpdatePassword";

        public AcountController(HttpClient client) : base(client) { }
        public async Task<ResponseDTO<Acount>> CheckLogin(Acount checkAcount)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(CheckLoginPath, checkAcount).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return DeserializeResponse<Acount>(response);
        }
        public async Task<ResponseDTO<object>> CreateAsync(Acount modelElement)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(AddCountPath,modelElement).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return DeserializeResponse<object>(response);
        }
        public async Task<ResponseDTO<Profile[]>> GetAcountProfiles(Acount targetAcount)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(GetAcountProfilesPath, targetAcount).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return DeserializeResponse <Profile[]>(response);
        }
        public async Task<ResponseDTO<object>> DeleteAsync(Acount targetAcount)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, DeleteAcountPath);
            HttpResponseMessage response = await SendRequest(request,targetAcount);

            return DeserializeResponse<object>(response);
        }
        public async Task<ResponseDTO<object>> UpdatePassword(Acount targetAcount, string newPassword)
        {
            ChangePassword passwordInfo = (ChangePassword)targetAcount;
            passwordInfo.NewPassword = newPassword;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, UpdatePasswordPath);
            HttpResponseMessage response = await SendRequest(request,passwordInfo);

            return DeserializeResponse<object>(response);
        }
    }
}
