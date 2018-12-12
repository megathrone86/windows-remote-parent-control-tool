using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRPCT.Properties;
using WRPCT.Server;

namespace WRPCT.Services
{
    public class MainService : IDisposable
    {
        static MainService instance;
        public static MainService Instance => instance ?? (instance = new MainService());

        WRPCTWebServer webServer;
        GamesMonitoringService gamesMonitoringService;

        public ControlService ControlService { get; private set; }

        MainService()
        {
            ControlService = new ControlService();
            gamesMonitoringService = new GamesMonitoringService();
            webServer = new WRPCTWebServer(Settings.Default.Port);
        }

        public void Start()
        {
            webServer.Start();
            gamesMonitoringService.Start();
        }

        public void Dispose()
        {
            webServer.Dispose();
            gamesMonitoringService.Dispose();
        }
    }
}
