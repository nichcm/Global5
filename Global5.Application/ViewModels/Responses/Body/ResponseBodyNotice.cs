namespace Global5.Application.ViewModels.Responses.Body
{
    public class ResponseBodyNotice
    {
        public ResponseBodyNotice(string codRespuesta, string descRespuesta)
        {
            pCodRespuesta = codRespuesta;
            pDescRespuesta = descRespuesta;
        }
        public string pCodRespuesta { get; set; }
        public string pDescRespuesta { get; set; }
    }
}