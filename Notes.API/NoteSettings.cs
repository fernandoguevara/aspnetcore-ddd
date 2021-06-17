using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API
{
    public class NoteSettings
    {
        public bool UseCustomizationData { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseVendor { get; set; }
        public string EventBusConnection { get; set; }
        public int GracePeriodTime { get; set; }
        public int CheckUpdateTime { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
