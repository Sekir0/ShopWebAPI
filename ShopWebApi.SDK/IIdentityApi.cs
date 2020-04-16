using System.Threading.Tasks;
using Refit;
using ShopWebApi.Contracts.V1.Responses.Pagination;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;

namespace ShopWebApi.SDK
{
    public interface IIdentityApi
    {
        [Post("/api/v1/identity/register")]
        Task<ApiResponse<PagedResponse<AuthSuccessResponse>>> RegisterAsynk([Body] UserRegistrationRequest registrationRequest);

        [Post("/api/v1/identity/login")]
        Task<ApiResponse<Response<AuthSuccessResponse>>> LoginAsynk([Body] UserLoginRequest loginRequest);

        [Post("/api/v1/identity/refresh")]
        Task<ApiResponse<Response<AuthSuccessResponse>>> TokenRefreshAsynk([Body] RefreshTokenRequest refreshRequest);
    }
}
