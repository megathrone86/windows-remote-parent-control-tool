using System;
using WRPCT.Helpers;
using static WRPCT.Helpers.ConfigHelper;

namespace WRPCT.BL
{
    public static class GamesConfigManager
    {
        static Config _params = ConfigHelper.Params;

        //static object _gamesTimeLeftLocker = new object();
        //static object _gamesEnabledLocker = new object();

        public static void ChangeGamesTime(TimeSpan delta)
        {
            lock (_params)
            {
                _params.GamesTimeLeft = _params.GamesTimeLeft.Add(delta);
                if (_params.GamesTimeLeft.TotalSeconds < 0)
                    _params.GamesTimeLeft = new TimeSpan(0);
                ConfigHelper.SaveConfig();
            }
        }

        public static void ChangeGamesEnabled(bool newValue)
        {
            lock (_params)
            {
                ConfigHelper.Params.AllowGames = newValue;
                ConfigHelper.SaveConfig();
            }
        }
    }
}
