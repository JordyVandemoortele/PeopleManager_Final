using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Vives.Authentication.Abstractions;
using Vives.Security.Jwt;

namespace PeopleManager.Ui.BlazorApp.Providers
{
    public class CustomAuthenticationStateProvider(ITokenStore tokenStore): AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Get bearer from TokenStore
            var token = await tokenStore.GetToken();

            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            if (!string.IsNullOrWhiteSpace(token))
            {
                //Convert token to ClaimsPrincipal
                principal = JwtSecurityHelper.GetClaimsPrincipal(token, "Jwt");
            }

            var state = new AuthenticationState(principal);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
    }
}
