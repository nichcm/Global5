using System.Net;

namespace Global5.Application.ViewModels
{
    public class ResponseWithError
    {
        public ResponseWithError(HttpStatusCode httpStatus, bool succes, string error)
        {
            HttpStatus = httpStatus;
            Success = succes;
            Error = error;
        }
        public HttpStatusCode HttpStatus { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}