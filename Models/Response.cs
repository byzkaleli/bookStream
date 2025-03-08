namespace bookStream.Models
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        // Yapıcı metod
        public Response(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        // Başarılı yanıt için yardımcı metot
        public static Response<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new Response<T>(true, data, message);
        }

        // Hata yanıtı için yardımcı metot
        public static Response<T> ErrorResponse(string message)
        {
            return new Response<T>(false, default, message);
        }
    }
}
