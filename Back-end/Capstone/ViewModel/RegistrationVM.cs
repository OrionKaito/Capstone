using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class RegistrationPaginVM
    {
        public int TotalPage { get; set; }
        public IEnumerable<RegistrationVM> Accounts { get; set; }
    }

    public class RegistrationVM
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ManagerID { get; set; }
        public IEnumerable<RoleVM> Roles { get; set; }
        public IEnumerable<GroupVM> Groups { get; set; }
    }

    public class RegistrationCM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ManagerID { get; set; }
        public IEnumerable<Guid> RoleIDs { get; set; }
        public IEnumerable<Guid> GroupIDs { get; set; }
    }

    public class RegistrationUM
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ManagerID { get; set; }
    }

    public class RegistrationByIDUM
    {
        public string ID { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ManagerID { get; set; }
        public IEnumerable<Guid> RoleIDs { get; set; }
        public IEnumerable<Guid> GroupIDs { get; set; }
    }
}
