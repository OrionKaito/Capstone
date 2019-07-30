using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class RegistrationPaginVM
    {
        public int TotalRecord { get; set; }
        public IEnumerable<RegistrationVM> Accounts { get; set; }
    }

    public class RegistrationVM
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public string LineManagerID { get; set; }
        public string ManagerName { get; set; }
        public RoleVM Role { get; set; }
        public IEnumerable<GroupVM> Groups { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RegistrationCM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string LineManagerID { get; set; }
        public Guid RoleID { get; set; }
        public IEnumerable<Guid> GroupIDs { get; set; }
    }

    public class RegistrationUM
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class RegistrationByAdminUM
    {
        public string ID { get; set; }
        public string LineManagerID { get; set; }
        public Guid RoleID { get; set; }
        public IEnumerable<Guid> GroupIDs { get; set; }
    }

    public class UpdateAvatar
    {
        public string ImagePath { get; set; }
    }
}
