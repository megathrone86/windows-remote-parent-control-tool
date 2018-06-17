using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRPCT.BL;
using WRPCT.Helpers;
using static WRPCT.Helpers.ConfigHelper;

namespace WRPCT.Services
{
    /// <summary>
    /// Сервис для управления поведением системы. Содержит основную бизнес-логику
    /// </summary>
    public class ControlService
    {
        Config _params = ConfigHelper.Params;

        public ControlService()
        {
        }

        public void EnableGames()
        {
            GamesConfigManager.ChangeGamesEnabled(true);
        }

        public void DisableGames()
        {
            GamesConfigManager.ChangeGamesEnabled(false);
        }

        public void UpdateTimeCounter(int minutes)
        {
            GamesConfigManager.ChangeGamesTime(TimeSpan.FromMinutes(minutes));
        }
    }
}
