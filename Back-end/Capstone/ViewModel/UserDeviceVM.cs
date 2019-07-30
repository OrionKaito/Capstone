using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
    public class UserDeviceVM
    {
        public string UserID { get; set; }
        public string DeviceToken { get; set; }
    }

    public class UserDeviceUM
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public string DeviceToken { get; set; }
    }
}
