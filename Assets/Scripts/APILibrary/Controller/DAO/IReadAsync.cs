using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;
using System.Threading.Tasks;

namespace ProyectXAPILibrary.Controller.DAO
{
    public interface IReadAsync<T> where T : Model
    {
        public Task<ResponseDTO<T>> ReadAsync(string id);
    }
}
