namespace Vives.Authentication.Abstractions
{
    public interface ITokenStore
    {
        Task<string?> GetToken();
        Task SaveToken(string token);
        Task ClearToken();
    }
}
