using System;
namespace IoT_Cloud.Models
{
    public class Response<T>
    {
        public Response()
        {
        }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; }
        public T Result { get; set; }

        public Response(string message, bool isSuccess = true)
        {
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}
