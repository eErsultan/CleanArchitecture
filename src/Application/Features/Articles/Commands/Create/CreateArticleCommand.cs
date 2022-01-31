using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Application.DTOs.Article;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;
using Application.Interfaces.Repositories.Base;

namespace Application.Features.Articles.Commands.Create
{
    public class CreateArticleCommand : IRequest<ArticleDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;

        public CreateArticleCommandHandler(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(request);
            await _repository.AddAsync(article);

            var dto = _mapper.Map<ArticleDto>(article);

            return dto;
        }
    }
}