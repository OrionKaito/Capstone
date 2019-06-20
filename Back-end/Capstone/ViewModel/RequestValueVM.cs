using System;

namespace Capstone.ViewModel
{

    public class RequestValueVM
    {
        public Guid ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid RequestActionID { get; set; }
    }

    public class RequestValueCM
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid RequestActionID { get; set; }
    }

    public class RequestValueUM
    {
        public Guid ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid RequestActionID { get; set; }
    }
}
