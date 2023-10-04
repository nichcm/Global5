using Global5.Application.ViewModels;
using System;

namespace Schedule.Application.ViewModels.Responses.Token
{
    public class TokenUserResponse : IResponse
    {
        public int UserLoginId { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
    }
}