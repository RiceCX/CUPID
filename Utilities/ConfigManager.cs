using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNet.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CUPID.Utilities {
    class ConfigManager {
        public enum Color { Red, Blue, Green };
        #region Glue-on properties
        //DotNet.Config will glue values from your config.properties 
        public string domainFile;
        public DomainFile Uploadconfiguration;
        #endregion


        public ConfigManager() {
            AppSettings.GlueOnto(this);
            readDomainFile();
        }
        private void readDomainFile() {
            using(StreamReader r = new StreamReader(domainFile)) {
                string json = r.ReadToEnd();
                Uploadconfiguration = JsonConvert.DeserializeObject<DomainFile>(json);
            }
        }
    }
    public class DomainFile {
        public string name;
        public string DestinationType;
        public string RequestURL;
        public string FileFormType;
        // argument please.
        public DomainFileArgs Arguments;
        // doesn't really need cause metadata
        public string URL;
        public string ThumbnailURL;
        public string DeletionURL;
    }
    public class DomainFileArgs {
        public string token;
    }
}
