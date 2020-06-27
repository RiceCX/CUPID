using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CUPID.Utilities {
    public class ServerResponse {
        public string url { get; set; }
        public string del_url { get; set; }
        public string thumb { get; set; }
        public static ServerResponse parse(byte[] json) {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<ServerResponse>(jsonStr);
        }
    }
}
