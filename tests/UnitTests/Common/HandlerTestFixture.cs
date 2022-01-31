using System;
using Application.Interfaces.Repositories;
using AutoMapper;
using Infrastructure.Persistence.Context;
using Application.Mappings;
using Infrastructure.Persistence.Repositories;
using Xunit;

namespace UnitTests.Common
{
    public class HandlerTestFixture<TService, TImplementation> : IDisposable
        where TImplementation : TService
    {
        private readonly ApplicationDbContext _context;
        public TService Repository { get; }
        public IMapper Mapper { get; }
        
        public HandlerTestFixture()
        {
            _context = DbContextFactory.CreateApplicationDbContext();
            
            var configurationProvider = new MapperConfiguration(config =>
            {
                config.AddProfile<GeneralProfile>();
            });
            Mapper = configurationProvider.CreateMapper();

            var type = typeof(TImplementation);
            Repository = (TService)Activator.CreateInstance(type, _context);
        }
        
        public void Dispose()
        {
            DbContextFactory.Destroy(_context);
        }
    }
    
    [CollectionDefinition("ArticleCollection")]
    public class ArticleCollection : ICollectionFixture<HandlerTestFixture<IArticleRepository, ArticleRepository>> { }
}