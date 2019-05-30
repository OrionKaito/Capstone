using System;

namespace Capstone.ViewModel
{
    public class NotificationVM
    {
        public Guid ID { get; set; }

        public Guid WorkFlowID { get; set; }

        public string Data { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class NotificationCM
    {
        public Guid WorkFlowID { get; set; }

        public string Data { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class NotificationUM
    {
        public Guid ID { get; set; }

        public Guid WorkFlowID { get; set; }

        public string Data { get; set; }
        public DateTime DateTime { get; set; }
    }
}
