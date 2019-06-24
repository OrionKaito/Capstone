﻿using System;

namespace Capstone.ViewModel
{
    public class PermissionOfGroupVM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class PermissionOfGroupCM
    {
        public Guid PermissionID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class PermissionOfGroupUM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public Guid GroupID { get; set; }
    }
}