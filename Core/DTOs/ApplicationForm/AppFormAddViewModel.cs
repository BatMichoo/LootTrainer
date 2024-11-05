using Infrastructure.Models.GuildApplications;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ApplicationForm
{
    public class AppFormAddViewModel
    {
        [Required]
        [RegularExpression("^[A-Z][a-z'-]{1,11}$")]
        public string CharacterName { get; set; } = null!;
        public CharacterClass Class { get; set; }
        public string Specialization { get; set; }
        public Professions Profession { get; set; }
        public int BattleNetId { get; set; }
        public int DiscordId { get; set; }
        public int RaidsPerWeek { get; set; }
        public string RaidRoles { get; set; }

        [Required]
        public Uri WarcraftLogs { get; set; } = null!;
        public string? ApplyReason { get; set; }
        public string? AboutYou { get; set; }
    }
}
