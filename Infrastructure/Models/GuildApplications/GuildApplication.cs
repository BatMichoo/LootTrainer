using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.GuildApplications
{
    public class GuildApplication : BaseEntity
    {
        [Required]
        [RegularExpression("^[A-Z][a-z'-]{1,11}$")]
        public string CharacterName { get; set; } = null!;

        [Required]
        public CharacterClass Class { get; set; }

        [Required]
        public string Specialization { get; set; } = null!;

        [Required]
        public string Profession { get; set; } = null!;

        public int? BattleNetId { get; set; }

        [Required]
        public int DiscordId { get; set; }

        [Required]
        public int RaidsPerWeek { get; set; }

        [Required]
        public string RaidRoles { get; set; } = null!;

        public byte[]? CharacterScreenshot { get; set; }

        [Required]
        public Uri WarcraftLogs { get; set; } = null!;

        public string? ApplyReason { get; set; }

        public string? AboutYou { get; set; }

        [Required]
        [ForeignKey(nameof(Applicant))]
        public string ApplicantId { get; set; } = null!;
        public BlizzUser Applicant { get; set; } = null!;

        [Required]
        public ApplicationState State { get; set; }
    }
}
