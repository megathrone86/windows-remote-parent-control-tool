using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WRPCT.Helpers
{
    public class ApplicationFolders
    {
        public static string AppData
        {
            get
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WRPCT");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return dir;
            }
        }

        public static string ConfigPath => Path.Combine(AppData, "config.json");
    }
}
