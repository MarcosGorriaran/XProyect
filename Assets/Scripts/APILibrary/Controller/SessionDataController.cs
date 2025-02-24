using System.Net.Http;
using System.Threading.Tasks;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using ProyectXAPILibrary.Controller.DAO;

namespace ProyectXAPILibrary.Controller
{
    public class SessionDataController : Controller, ICreateMultipleAsync<SessionData>, IReadListAsync<SessionData>
    {
        const string ProfileIDName = "id=";
        const string ProfileCreatorName = "creatorName=";
        const string SessionIDName = "sessionID=";

        const string AddSessionDataPath = "AddSessionData";
        const string GetAllSessionDataPath = "GetAllSessionData";
        const string GetProfileSessionDataPath = "GetProfileSessionData";
        const string GetSessionDataInfoPath = "GetSessionDataInfo";
        const string GetSessionDataPath = "GetSessionData";
        public SessionDataController(HttpClient client) : base(client) { }

        public async Task<ResponseDTO<object>> CreateMultipleAsync(SessionData[] dataList)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AddSessionDataPath);
            HttpResponseMessage response = await SendRequest(request, dataList);

            return DeserializeResponse<object>(response);
        }
        public async Task<ResponseDTO<SessionData[]>> ReadAllAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GetAllSessionDataPath);
            HttpResponseMessage response = await SendRequest(request);

            return DeserializeResponse<SessionData[]>(response);
        }
        public async Task<ResponseDTO<SessionData[]>> ReadProfileSessionDataAsync(int id, string creatorName)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GetProfileSessionDataPath+"?"+ProfileIDName+id+ "&"+ProfileCreatorName+creatorName);
            HttpResponseMessage response = await SendRequest(request);

            return DeserializeResponse<SessionData[]>(response);
        }
        public async Task<ResponseDTO<SessionData[]>> ReadSessionDataInfo(int sessionID)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GetProfileSessionDataPath + "?" + SessionIDName+sessionID);
            HttpResponseMessage response = await SendRequest(request);

            return DeserializeResponse<SessionData[]>(response);
        }
        public async Task<ResponseDTO<SessionData>> ReadSessionData(int profileId, string creatorName, int sessionID)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, GetSessionDataPath + "?" + ProfileIDName + profileId + "&" + ProfileCreatorName + creatorName+"&"+SessionIDName+sessionID);
            HttpResponseMessage response = await SendRequest(request);

            return DeserializeResponse<SessionData>(response);
        }
    }
}
