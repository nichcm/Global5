using System.Net;

namespace Global5.Application.ViewModels
{
    public class ResponseBody
    {
        public ResponseBody(HttpStatusCode httpStatus, bool succes, object result, string[] error)
        {
            HttpStatus = httpStatus;
            Success = succes;
            Result = result;
            Error = error;
        }
        public HttpStatusCode HttpStatus { get; set; }
        public bool Success { get; set; }
        public object Result { get; set; }
        public string[] Error { get; set; }
    }
}