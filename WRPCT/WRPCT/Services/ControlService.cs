using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPCT.Services
{
    /// <summary>
    /// Сервис для управления поведением системы. Содержит основную бизнес-логику
    /// </summary>
    public class ControlService
    {
        /// <summary>
        /// Вызывается, когда пользователь изменяет что-то в настройках системы
        /// </summary>
        public event EventHandler ParametersUpdated;

        public bool AllowGames { get; set; }
        public TimeSpan GamesTimeLeft { get; set; }

        public ControlService()
        {
            AllowGames = false;
            GamesTimeLeft = TimeSpan.FromMinutes(120);
        }

        public void EnableGames()
        {
            AllowGames = true;
        }

        public void DisableGames()
        {
            AllowGames = false;
        }

        public void UpdateTimeCounter(int minutes)
        {
            var deltaTimeSpan = TimeSpan.FromMinutes(minutes);
            GamesTimeLeft = GamesTimeLeft.Add(deltaTimeSpan);
            if (GamesTimeLeft.TotalSeconds < 0)
                GamesTimeLeft = new TimeSpan(0);
        }
    }
}
