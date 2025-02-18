using System.Threading.Tasks;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;

namespace ProyectXAPILibrary.Controller.DAO
{
    public interface IReadListAsync<T> where T : Model
    {
        public Task<ResponseDTO<T[]>> ReadAllAsync();
    }
}
