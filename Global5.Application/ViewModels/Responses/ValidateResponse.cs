using System;

namespace Global5.Application.ViewModels.Responses
{
    public class ValidateResponse : IResponse
    {
        public ValidateResponse(bool isValid, string message, Guid id)
        {
            IsValid = isValid;
            Message = message;
            Id = id;
        }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public Guid Id { get; set; }
    }
}
