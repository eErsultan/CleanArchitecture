using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Features.Articles.Commands.Update
{
    public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        private readonly IArticleRepository _repository;
        
        public UpdateArticleCommandValidator(IArticleRepository repository)
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