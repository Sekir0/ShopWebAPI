using Refit;
using System.Threading.Tasks;
using ShopWebAPI.Contracts.V1.Requests;

namespace ShopWebApi.SDK.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cashedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");
            var shopWebApi  = RestService.For<IShopWebApi>("https://localhost:5001", new RefitSettings 
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cashedToken)
            });

            var registerResponse = await identityApi.RegisterAsynk(new UserRegistrationRequest
            {
                Email = "SDK@sekiro.com",
                Password = "Qwer!234"
            });

            var loginResponse = await identityApi.LoginAsynk(new UserLoginRequest
            {
                Email = "SDK@sekiro.com",
                Password = "Qwer!234"
            });

            cashedToken = loginResponse.Content.Token;

            var allProduts = await shopWebApi.GetAllAsynk();

            var createdProduct = await shopWebApi.CreateAsynk(new CreateProductRequest 
            {
                Name = "SdkProduct",
                Description = "Test SDK",
                Price = 10,
                Quantity = 1,
                Url = "Some Url",
                Categorys = new[] {"SDK"}
            });

            var getByIdProduct = await shopWebApi.GetAsynk(createdProduct.Content.Id);

            var updateProduct = await shopWebApi.UpdateAsynk(createdProduct.Content.Id, new UpdateProductRequest 
            {
                Name = "UpdatedSDKProduct",
                Description = "Test Update method",
                Price = 9,
                Quantity = 2,
                Url = "Some updated url"
            });

            var deleteProduct = await shopWebApi.DeleteAsynk(createdProduct.Content.Id);
        }
    }
}
