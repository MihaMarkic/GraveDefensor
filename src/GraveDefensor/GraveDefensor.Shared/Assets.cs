using Settings = GraveDefensor.Engine.Settings;
using System;

namespace GraveDefensor.Shared
{
    public static class Assets
    {
        public static class Weapons
        {
            public const string MiniGun = "Weapons/MiniGun";
            public const string Vulcan = "Weapons/Vulcan";

            public static string FromSettings(Settings.Weapon weapon)
            {
                switch (weapon)
                {
                    case Settings.MiniGun _: return MiniGun;
                    case Settings.Vulcan _: return Vulcan;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(weapon));
                }
            }
        }
        public static class Fonts
        {
            public const string WeaponDescriptionFont = "Fonts/Hud";
        }
    }
}
