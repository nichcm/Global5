using System.Net;

namespace Crdc.Agro.Plataforma.Worker.Models
{
    public class ResponseBody
    {
        public ResponseBody(HttpStatusCode httpStatus, bool succes, object result)
        {
            HttpStatus = httpStatus;
            Success = succes;
            Result = result;
        }
        public HttpStatusCode HttpStatus { get; set; }
        public bool Success { get; set; }
        public object Result { get; set; }
    }
}