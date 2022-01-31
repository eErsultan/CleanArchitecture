using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.Article;
using FluentAssertions;
using IntegrationTests.Helpers;
using IntegrationTests.Host;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence.Context;
using Xunit;
using IntegrationTests.Seeds;
using Newtonsoft.Json;
using Xunit.Priority;
using Application.Extensions;
using Application.Features.Articles.Commands.Create;
using Application.Features.Articles.Commands.Update;
using Application.Features.Articles.Queries.GetAll;

namespace IntegrationTests.Controllers
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ArticlesControllerTests : IClassFixture<CrmWebAppFactory>
    {
        private readonly HttpClient _client;

        public ArticlesControllerTests(CrmWebAppFactory factory)
        {
            _client = factory.WithWebHostBuilder(builder => 
            {
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    try
                    {
                        context.Database.EnsureCreated();
                        DefaultArticles.Seed(context);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                });
            }).CreateClient();
        }
        
        #region GetAll

        [Theory]
        [Priority(1)]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        public async Task GetAll_ParamsInQuery_ReturnOkStatusCode(int pageNumber, int pageSize)
        {
            // Arrange
            //await AuthenticateAsync();
            var query = new GetAllArticlesQuery()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            
            // Act
            var httpResponse = await _client.GetAsync(
                ApiRoutes.Article.GetAll
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .AddQuery<GetAllArticlesQuery>(query));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ICollection<ArticleDto>>(stringResponse);
            response.Should().BeOfType(typeof(List<ArticleDto>));

            if (pageSize == 0)
            {
                response.Count.Should().Be(3);
            }
            else
            {
                response.Count.Should().Be(pageSize);
            }
            
        }

        #endregion


        #region GetById

        [Fact]
        public async Task GetById_IdInRoute_ReturnOkStatusCode()
        {
            // Arrange
            //await AuthenticateAsync();

            var id = 1;

            // Act
            var httpResponse = await _client.GetAsync(
                ApiRoutes.Article.GetById
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ArticleDto>(stringResponse);
            response.Should().BeOfType(typeof(ArticleDto));
            response.Id.Should().Be(id);
            response.Name.Should().Be("Article 1");
            response.Description.Should().Be("Description 1");
        }

        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async Task GetById_IdEntityInRoute_ReturnNotFoundStatusCode(int id)
        {
            // Arrange
            //await AuthenticateAsync();

            // Act
            var httpResponse = await _client.GetAsync(
                ApiRoutes.Article.GetById
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Should().BeOfType(typeof(ErrorDetails));
            response.Message.Should().Be("Статья не найдена");
        }

        #endregion


        #region Create

        [Fact]
        public async Task Create_DtoInBody_ReturnOkStatusCode()
        {
            // Arrange
            //await AuthenticateAsync();

            var dto = new CreateArticleCommand()
            {
                Name = "Create article test",
                Description = "Description empty"
            };

            // Act
            var httpResponse = await _client.PostAsync(
                ApiRoutes.Article.Create.Replace(ApiVersion.Name, ApiVersion.V1),
                new JsonContent(dto));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ArticleDto>(stringResponse);
            response.Should().BeOfType(typeof(ArticleDto));
            response.Name.Should().Be(dto.Name);
            response.Description.Should().Be(dto.Description);
        }

        [Fact]
        public async Task Create_DtoWithoutNameInBody_ReturnBadRequestStatusCode()
        {
            // Arrange
            //await AuthenticateAsync();

            var dto = new CreateArticleCommand()
            {
                Description = "Description empty"
            };

            // Act
            var httpResponse = await _client.PostAsync(
                ApiRoutes.Article.Create.Replace(ApiVersion.Name, ApiVersion.V1),
                new JsonContent(dto));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Should().BeOfType(typeof(ErrorDetails));
            response.Message.Should().Be("Name является обязательным");
        }

        #endregion


        #region Delete

        [Fact]
        public async Task Delete_IdInRoute_ReturnOkStatusCode()
        {
            // Arrange
            //await AuthenticateAsync();

            var id = 2;

            // Act
            var httpResponse = await _client.DeleteAsync(
                ApiRoutes.Article.Delete
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<int>(stringResponse);
            response.Should().BeOfType(typeof(int));
            response.Should().Be(id);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async Task Delete_IdEntityInRoute_ReturnNotFoundStatusCode(int id)
        {
            // Arrange
            //await AuthenticateAsync();

            // Act
            var httpResponse = await _client.DeleteAsync(
                ApiRoutes.Article.Delete
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Should().BeOfType(typeof(ErrorDetails));
            response.Message.Should().Be("Статья не найдена");
        }

        #endregion


        #region Update

        [Fact]
        public async Task Update_IdInRouteAndDtoInBody_ReturnOkStatusCode()
        {
            // Arrange
            //await AuthenticateAsync();

            var id = 3;
            var dto = new UpdateArticleCommand()
            {
                Name = "Update Article Name",
                Description = "Update description article"
            };

            // Act
            var httpResponse = await _client.PutAsync(
                ApiRoutes.Article.Update
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()),
                new JsonContent(dto));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ArticleDto>(stringResponse);
            response.Should().BeOfType(typeof(ArticleDto));
            response.Id.Should().Be(id);
            response.Name.Should().Be(dto.Name);
            response.Description.Should().Be(dto.Description);
        }


        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async Task Update_IdEntityInRouteAndDtoInBody_ReturnNotFoundStatusCode(int id)
        {
            // Arrange
            //await AuthenticateAsync();

            var dto = new UpdateArticleCommand()
            {
                Name = "Update Article Name",
                Description = "Update description article"
            };

            // Act
            var httpResponse = await _client.PutAsync(
                ApiRoutes.Article.Update
                    .Replace(ApiVersion.Name, ApiVersion.V1)
                    .Replace(ApiRoutes.Article.ArticleId, id.ToString()),
                new JsonContent(dto));

            // Assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ErrorDetails>(stringResponse);
            response.Should().BeOfType(typeof(ErrorDetails));
            response.Message.Should().Be("Статья не найдена");
        }

        #endregion
    }
}