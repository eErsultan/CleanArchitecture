using System.Net;
using FluentValidation;

namespace Application.Features.Articles.Commands.Create
{
    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} является обязательным")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString())
                .NotNull();    
        }
    }
}