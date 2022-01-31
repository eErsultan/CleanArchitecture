using System.Collections.Generic;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace IntegrationTests.Seeds
{
    public static class DefaultArticles
    {
        public static void Seed(ApplicationDbContext context) 
        {
            var articles = new List<Article>()
            {
                new Article()
                {
                    Id = 1,
                    Name = "Article 1",
                    Description = "Description 1",
                    IsDeleted = false
                },
                new Article()
                {
                    Id = 2,
                    Name = "Article 2",
                    Description = "Description 2",
                    IsDeleted = false
                },
                new Article()
                {
                    Id = 3,
                    Name = "Article 3",
                    Description = "Description 3",
                    IsDeleted = false
                },
                new Article()
                {
                    Id = 4,
                    Name = "Article 4",
                    Description = "Description 4",
                    IsDeleted = true
                }
            };

            context.Articles.AddRangeAsync(articles);
            context.SaveChangesAsync();
        }
    }
}