using System.Threading;
using System.Threading.Tasks;
using Application.Features.Articles.Commands.Create;
using Application.Interfaces.Repositories;
using AutoMapper;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using UnitTests.Common;
using Xunit;

namespace UnitTests.Handlers.Articles.Commands
{
    [Collection("ArticleCollection")]
    public class CreateArticleCommandTests
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;
        
        public CreateArticleCommandTests(HandlerTestFixture<IArticleRepository, ArticleRepository> fixture)
        {
            _repository = fixture.Repository;
            _mapper = fixture.Mapper;
        }
        
        [Fact]
        public async Task CreateArticleCommandHandler_Success()
        {
            // Arrange
            var handler = new CreateArticleCommandHandler(_repository, _mapper);
            var name = "Article";
            var description = "Description";
            
            // Act
            var result = await handler.Handle(
                new CreateArticleCommand()
                {
                    Name = name,
                    Description = description
                }, 
                CancellationToken.None);
            
            // Assert
            result.Name.Should().Be(name);
            result.Description.Should().Be(description);

            var article = await _repository.GetByIdAsync(result.Id);
            article.Name.Should().Be(name);
            article.Description.Should().Be(description);
        }
    }
}