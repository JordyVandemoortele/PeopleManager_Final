using System.Runtime.CompilerServices;
using Blazored.LocalStorage;
using Vives.Authentication.Abstractions;

namespace PeopleManager.Ui.BlazorApp.Stores
{
    public class TokenStore(ILocalStorageService localStorageService) :ITokenStore
    {
        private const string TokenName = "BearerToken";

        public async Task<string?> GetToken()
        {
            return await localStorageService.GetItemAsStringAsync(TokenName);
        }

        public async Task SaveToken(string token)
        {
            await localStorageService.SetItemAsStringAsync(TokenName, token);
        }

        public async Task ClearToken()
        {
            var itemExists = await localStorageService.ContainKeyAsync(TokenName);
            if (itemExists)
            {
                await localStorageService.RemoveItemAsync(TokenName);
            }
        }
    }
}
