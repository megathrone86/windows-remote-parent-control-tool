using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            webServer = new WRPCTWebServer(5400);
            gamesMonitoringService = new GamesMonitoringService();
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
