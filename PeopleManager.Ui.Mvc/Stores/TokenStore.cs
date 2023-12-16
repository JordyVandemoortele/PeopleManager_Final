using Vives.Authentication.Abstractions;

namespace PeopleManager.Ui.Mvc.Stores
{
    public class TokenStore(IHttpContextAccessor contextAccessor) : ITokenStore
    {
        private const string JwtTokenName = "JwtToken";

        public Task<string?> GetToken()
        {
            string? jwtToken = null;
            contextAccessor.HttpContext?.Request.Cookies.TryGetValue(JwtTokenName, out jwtToken);
            return Task.FromResult(jwtToken);
        }

        public Task SaveToken(string token)
        {
            ClearToken();
            contextAccessor.HttpContext?.Response.Cookies.Append(JwtTokenName, token, new CookieOptions{HttpOnly = true});
            return Task.CompletedTask;
        }

        public Task ClearToken()
        {
            contextAccessor.HttpContext?.Response.Cookies.Delete(JwtTokenName);
            return Task.CompletedTask;
        }
    }
}
