using Core.Repositories.BaseRepository;
using Infrastructure;
using Infrastructure.Models;

namespace Core.Repositories.GuildApplicationRepo
{
    public class ApplicationRepository : Repository<GuildApplication>, IApplicationRepository
    {
        public ApplicationRepository(LootDbContext dbContext) : base(dbContext)
        {
        }
    }
}
