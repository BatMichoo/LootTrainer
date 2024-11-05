using Core.Repositories.BaseRepository;
using Infrastructure;
using Infrastructure.Models.GuildApplications;

namespace UnitTests.Repository
{
    public class TestRepository : Repository<GuildApplication>
    {
        public TestRepository(LootDbContext dbContext) : base(dbContext)
        {
        }
    }
}
