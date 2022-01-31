using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Article;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Articles.Commands.Update
{
    public class UpdateArticleCommand : IRequest<ArticleDto>
    {
        [JsonIgnore]
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
    {
        private readonly IArticleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateArticleCommandHandler(IArticleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetByIdAsync(request.ArticleId);
            article = _mapper.Map(request, article);
            await _repository.UpdateAsync(article);
            
            var dto = _mapper.Map<ArticleDto>(article);

            return dto;
        }
    }
}