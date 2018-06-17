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
        private static Config _params;
        public static Config Params
        {
            get
            {
                return _params ?? (_params = LoadConfig());
            }
        }

        static Config LoadConfig()
        {
            try
            {
                using (FileStream fs = new FileStream(ApplicationFolders.ConfigPath, FileMode.OpenOrCreate))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Config));
                    var result = serializer.ReadObject(fs);
                    return (Config)result;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Config();
            }
        }

        public static void SaveConfig()
        {
            lock (Params)
            {
                using (FileStream fs = new FileStream(ApplicationFolders.ConfigPath, FileMode.Create))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Config));
                    serializer.WriteObject(fs, Params);
                }
            }
        }

        public class Config
        {
            public string Password { get; set; } = "123456";
            public int Port { get; set; } = 5400;
            public TimeSpan GamesTimeLeft { get; set; }
            public bool AllowGames { get; set; }
        }
    }
}
