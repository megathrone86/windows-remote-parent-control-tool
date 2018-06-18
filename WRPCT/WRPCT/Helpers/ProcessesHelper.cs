using System.Collections.Generic;
using System.Diagnostics;

namespace WRPCT.Helpers
{
    public class ProcessesHelper
    {
        public static IEnumerable<string> GetProcesses()
        {
            var allProcesses = Process.GetProcesses();
            foreach (var process in allProcesses)
            {
                string filePath = null;
                try
                {
                    filePath = process.MainModule.FileName;
                } catch { }
                if (filePath != null)
                    yield return filePath;
            }
        }
    }
}
