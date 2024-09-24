namespace Core.Services.BattleNet
{
    public static class Endpoints
    {
        // Base addresses for blizzard Apis
        public static class Base
        {
            public const string OAuth = "https://oauth.battle.net/";

            /// <summary>
            /// Please choose a region when using.
            /// </summary>
            public const string BlizzardAPI = "https://{0}.api.blizzard.com/";
        }

        //Profile related endpoints
        public static class Profile
        {
            public const string Summary = "profile/user/wow";
        }

        // Account related endpoints
        public static class Account
        {
            public const string Summary = "account/{0}";
        }

        // Character related endpoints
        public static class Character
        {
            /// <summary>
            /// First slot is for realm slug, second for character name.
            /// </summary>
            public const string Detail = "character/{0}/{1}";

            /// <summary>
            /// First slot is for realm slug, second for character name.
            /// </summary>
            public const string Protected = "character/{0}/{1}/protected";
        }

        // Playable Class related endpoints
        public static class Class
        {
            /// <summary>
            /// Class Id required.
            /// </summary>
            public const string PlayableClass = "playable-class/{0}";
        }

        // Playable Race related endpoints
        public static class Race
        {
            /// <summary>
            /// Race Id required.
            /// </summary>
            public const string PlayableRace = "playable-race/{0}";
        }

        // Realm related endpoints
        public static class Realm
        {
            /// <summary>
            /// Realm slug required.
            /// </summary>
            public const string Details = "realm/{0}";
        }        
    }
}
