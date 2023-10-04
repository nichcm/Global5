using System;

namespace Global5.Application.ViewModels.Responses.Token
{
    public class TokenCreateResponse : IResponse
    {
        public TokenCreateResponse(string accessToken = "", string refreshToken = "", string message = "", bool isValid = false)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Message = message;
            IsValid = isValid;
        }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public string Message { get; private set; }
        public bool IsValid { get; private set; }
    }
}