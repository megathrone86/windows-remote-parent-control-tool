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

        WRPCTWebServer webServer = new WRPCTWebServer();

        public ControlService ControlService { get; private set; }

        MainService()
        {
            ControlService = new ControlService();
        }

        public void Start()
        {
            webServer.Start(5400);
        }

        public void Dispose()
        {
            webServer.Dispose();
        }
    }
}
