using Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.ApplicationForm
{
    public class AppFormAddViewModel
    {
        [RegularExpression("^[A-Z][a-z'-]{1,11}$")]
        public string CharacterName { get; set; }
        public CharacterClass Class { get; set; }
        public string Specialization { get; set; }
        public string Profession { get; set; }
        public int BattleNetId { get; set; }
        public int DiscordId { get; set; }
        public int RaidsPerWeek { get; set; }
        public string RaidRoles { get; set; }
        public string WarcraftLogs { get; set; }
        public string ApplyReason { get; set; }
        public string AboutYou { get; set; }
    }
}
