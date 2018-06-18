using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WRPCT.Helpers;
using static WRPCT.Helpers.ConfigHelper;

namespace WRPCT.Services
{
    public class GamesMonitoringService : IDisposable
    {
        Config _params = ConfigHelper.Params;
        AsyncTaskManager asyncTaskManager;

        public GamesMonitoringService()
        {
            asyncTaskManager = new AsyncTaskManager(async (cancellationToken) =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(5000);
                        CheckProcesses();
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });
        }

        public void Start()
        {
            asyncTaskManager.Start();
        }

        void CheckProcesses()
        {
            var processes = ProcessesHelper.GetProcesses();
        }

        public void Dispose()
        {
            asyncTaskManager.Dispose();
        }
    }
}
