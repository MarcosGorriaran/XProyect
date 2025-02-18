namespace ProyectXAPI.Models.DTO
{
    public class ResponseDTO<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; } = "";
    }
}
