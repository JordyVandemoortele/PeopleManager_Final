using PeopleManager.Sdk.Extensions;
using Vives.Authentication.Abstractions;

namespace PeopleManager.Sdk.Handlers
{
    public class AuthorizationHttpHandler(ITokenStore tokenStore) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await tokenStore.GetToken();

            request.AddAuthorization(token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
