using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ShopWebApi.Contracts.V1.Responses.Pagination;
using ShopWebAPI.Contracts;
using ShopWebAPI.Contracts.V1.Requests;
using ShopWebAPI.Contracts.V1.Responses;
using FluentAssertions;
using Xunit;

namespace ShopWebApi.IntegrationTests
{
    public class ProductControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyProducts_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Products.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<PagedResponse<ProductResponse>>()).Data.Should().BeEmpty();
        }
        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createdProduct = await CreatePostAsync(new CreateProductRequest
            {
                Name = "Test prod",
                Price = 10,
                Quantity = 5,
                Description = "Test desc",
                Url = "Some url",
                Categorys = new[] { "testCategorys" }
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Products.GetById.Replace("{postId}", createdProduct.Id.ToString()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedProduct = await response.Content.ReadAsAsync<Response<ProductResponse>>();
            returnedProduct.Data.Id.Should().Be(createdProduct.Id);
            returnedProduct.Data.Name.Should().Be("Test prod");
            returnedProduct.Data.Price.Should().Be(10);
            returnedProduct.Data.Quantity.Should().Be(5);
            returnedProduct.Data.Description.Should().Be("Test desc");
            returnedProduct.Data.Url.Should().Be("Some url");
            returnedProduct.Data.Categorys.Single().Name.Should().Be("testCategorys");
        }
    }
}
