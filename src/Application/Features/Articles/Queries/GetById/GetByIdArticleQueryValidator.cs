using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Articles.Queries.GetById
{
    public class GetByIdArticlesQueryValidator : AbstractValidator<GetByIdArticleQuery>
    {
        private readonly IArticleRepository _repository;

        public GetByIdArticlesQueryValidator(IArticleRepository repository)
        {
            _repository = repository;
            
            RuleFor(x => x.ArticleId)
                .MustAsync(IsExist).WithMessage("Статья не найдена")
                .WithErrorCode(HttpStatusCode.NotFound.ToString());
        }
        
        private async Task<bool> IsExist(int id, CancellationToken token)
        {
            return await _repository.IsExistAsync(id);
        }
    }
}