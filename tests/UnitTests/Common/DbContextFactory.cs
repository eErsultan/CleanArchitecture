using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Context;
using Infrastructure.Shared.Services;
using UnitTests.Common.Services;

namespace UnitTests.Common
{
    public static class DbContextFactory
    {
        public static ApplicationDbContext CreateApplicationDbContext()
        {
            var authenticatedUserService = new AuthenticatedUserService();
            var dateTimeService = new DateTimeService();
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryApplicationDb")
                .Options;

            var context = new ApplicationDbContext(options, authenticatedUserService, dateTimeService);

            context.Database.EnsureCreated();
            context.Articles.AddRange(
                new Article()
                {
                    Id = 1,
                    Name = "Article 1",
                    Description = "Description 1"
                },
                new Article()
                {
                    Id = 2,
                    Name = "Article 2",
                    Description = "Description 2"
                },
                new Article()
                {
                    Id = 3,
                    Name = "Article 3",
                    Description = "Description 3"
                },
                new Article()
                {
                    Id = 4,
                    Name = "Article 4",
                    Description = "Description 4"
                });
            
            context.SaveChangesAsync();
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}