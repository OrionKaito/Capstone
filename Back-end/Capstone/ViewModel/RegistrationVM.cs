namespace Capstone.ViewModel
{
    public class RegistrationVM
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RegistrationCM
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }

    public class RegistrationUM
    {
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
