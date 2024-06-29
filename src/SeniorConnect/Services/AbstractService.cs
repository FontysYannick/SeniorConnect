using System.Net.Http.Headers;
using System.Security.Authentication;

namespace SeniorConnect.Services;

public abstract class AbstractService
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected AbstractService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    protected HttpClient GetAuthorizedClient()
    {
        var client = _httpClientFactory.CreateClient("SeniorConnectAPI");
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];

        if (token == null)
        {
            throw new AuthenticationException("Missing JWT token");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    protected HttpClient GetClient()
    {
        return _httpClientFactory.CreateClient("SeniorConnectAPI");
    }
}