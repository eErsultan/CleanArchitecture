using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IAuthenticatedUserService authenticatedUser,
            IDateTimeService dateTime) : base(options)
        {
            _authenticatedUser = authenticatedUser;
            _dateTime = dateTime;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity<int>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateDate = _dateTime.Now;
                        entry.Entity.CreatedUserId = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdateDate = _dateTime.Now;
                        entry.Entity.UpdatedUserId = _authenticatedUser.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        
        public DbSet<Article> Articles { get; set; }
    }
}
