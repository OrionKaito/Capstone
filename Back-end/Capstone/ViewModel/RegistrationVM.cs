using System;

namespace Capstone.ViewModel
{
    public class RegistrationVM
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid ManagerID { get; set; }
    }

    public class RegistrationCM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? ManagerID { get; set; }
    }

    public class RegistrationUM
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid ManagerID { get; set; }
    }
}
