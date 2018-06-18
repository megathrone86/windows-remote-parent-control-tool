using System;
using System.Threading;
using System.Threading.Tasks;

namespace WRPCT.Helpers
{
    public class AsyncTaskManager : IDisposable
    {
        Task task;
        CancellationTokenSource ctSource;

        public AsyncTaskManager(Action<CancellationToken> action)
        {
            ctSource = new CancellationTokenSource();
            task = new Task(() =>
            {
                action.Invoke(ctSource.Token);
            }, ctSource.Token);
        }

        public void Start()
        {
            task.Start();
        }

        public void Stop()
        {
            ctSource?.Cancel();
            task?.Wait();
        }

        public void Dispose()
        {
            Stop();
            ctSource.Dispose();
            task.Dispose();
        }
    }
}
