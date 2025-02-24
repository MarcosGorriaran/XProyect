
using System.Threading.Tasks;
using ProyectXAPI.Models;
using ProyectXAPI.Models.DTO;

namespace ProyectXAPILibrary.Controller.DAO
{
    public interface ICreateMultipleAsync<T> where T : Model
    {
        public Task<ResponseDTO<object>> CreateMultipleAsync(T[] modelList);
    }
}
