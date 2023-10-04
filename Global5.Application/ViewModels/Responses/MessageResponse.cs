namespace Global5.Application.ViewModels.Responses
{
    public class MessageResponse
    {
        public MessageResponse(string message = "Traducción no encontrada.")
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}