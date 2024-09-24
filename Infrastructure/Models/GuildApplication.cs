using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class GuildApplication : BaseEntity
    {
        public string CharacterName { get; set; }
        public CharacterClass Class { get; set; }
        public string Specialization { get; set; }
        public string Profession { get; set; }
        public string BattleNetId { get; set; }
        public string DiscordId { get; set; }
        public int RaidsPerWeek { get; set; }
        public string RaidRoles { get; set; }
        public byte[] CharacterScreenshot { get; set; }
        public string WarcraftLogs { get; set; }
        public string ApplyReason { get; set; }
        public string AboutYou { get; set; }

        [Required]
        [ForeignKey(nameof(Applicant))]
        public string ApplicantId { get; set; } = null!;
        public BlizzUser Applicant { get; set; } = null!;
    }
}
