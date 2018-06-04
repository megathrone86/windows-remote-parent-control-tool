using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WRPCT.Helpers
{
    public class ResourceHelper
    {
        public static string GetStringFromEmdeddedResource(string resourceName)
        {
            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            if (!resources.Contains(resourceName))
                return null;

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
