using Infrastructure.Models.GuildApplications;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models
{
    public class BlizzUser : IdentityUser
    {        
        public GuildApplication? GuildApplication { get; set; }
    }
}
