﻿using Infrastructure.Models;

namespace Core.DTOs.ApplicationForm
{
    public class AppFormModel
    {
        public int Id { get; set; }
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
    }
}