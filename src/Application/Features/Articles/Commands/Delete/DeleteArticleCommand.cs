using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Articles.Commands.Delete
{
    public class DeleteArticleCommand : IRequest<int>
    {
        public int ArticleId { get; set; }
    }

    class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, int>
    {
        private readonly IArticleRepository _repository;

        public DeleteArticleCommandHandler(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.ArticleId);
            await _repository.DeleteAsync(entity);
            
            return entity.Id;
        }
    }
}