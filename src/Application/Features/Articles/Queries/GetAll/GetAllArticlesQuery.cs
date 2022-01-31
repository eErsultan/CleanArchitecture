using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Article;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Articles.Queries.GetAll
{
    public class GetAllArticlesQuery : IRequest<ICollection<ArticleDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    class GetAllArticlesQueryHandler : IRequestHandler<GetAllArticlesQuery, ICollection<ArticleDto>>
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllArticlesQueryHandler(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {
            var count = await _repository.CountAsync();
            if (request.PageNumber == 0 && request.PageSize == 0)
            {
                request.PageNumber = 1;
                request.PageSize = count;
            }

            var articles = await _repository.GetAllAsync(request.PageNumber, request.PageSize);
            var dtos = _mapper.Map<ICollection<ArticleDto>>(articles.ToList());
            
            return dtos;
        }
    }
}