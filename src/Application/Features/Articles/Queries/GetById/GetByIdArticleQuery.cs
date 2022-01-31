using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Article;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Articles.Queries.GetById
{
    public class GetByIdArticleQuery : IRequest<ArticleDto>
    {
        public int ArticleId { get; set; }
    }

    class GetByIdArticleQueryHandler : IRequestHandler<GetByIdArticleQuery, ArticleDto>
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;
        
        public GetByIdArticleQueryHandler(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetByIdArticleQuery request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetByIdAsync(request.ArticleId);
            var dto = _mapper.Map<ArticleDto>(article);

            return dto;
        }
    }
}