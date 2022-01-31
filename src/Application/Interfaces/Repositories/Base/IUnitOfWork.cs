namespace Application.Interfaces.Repositories.Base
{
    public interface IUnitOfWork
    {
        IDatabaseTransaction BeginTransaction();
        public IArticleRepository Articles { get; }
    }
}
