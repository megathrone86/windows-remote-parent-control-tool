using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WRPCT.Helpers
{
    public class ConfigHelper
    {
        private Config _params;
        public Config Params
        {
            get
            {
                return _params ?? (_params = LoadConfig());
            }
        }

        Config LoadConfig()
        {
            using (FileStream fs = new FileStream(ApplicationFolders.ConfigPath, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ConfigHelper));
                return serializer.ReadObject(fs) as Config ?? new Config();
            }
        }

        public void SaveConfig()
        {
            using (FileStream fs = new FileStream(ApplicationFolders.ConfigPath, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ConfigHelper));
                serializer.WriteObject(fs, Params);
            }
        }

        public class Config
        {
            public string Password { get; set; } = "123456";
        }
    }
}
