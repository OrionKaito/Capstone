namespace Capstone.Helper
{
    public class WebConstant
    {
        //Variables
        public static int DefaultPageRecordCount = 10;

        //Variables's name
        public static string Permissions = "permissions";
        public static string Roles = "roles";
        public static string Groups = "groups";
        public static string Resources = "Resources";

        //Return messages
        public static string Success = "Success.";
        public static string NotFound = "ID not found!";
        public static string AccessDined = "Access Denied! You don't have permission to execute this action.";
        public static string NameExisted = "'s name is existed!";
        public static string InvalidUSer = "Invalid username or password!";
        public static string User = "user";
        public static string Admin = "admin";
        public static string Staff = "staff";
        public static string VerifyAccount = "Please verify your account first!";
        public static string BannedAccount = "Account is banned!";
        public static string NoNotificationYet = "There are not any notfication yet!";
        public static string NoRequestYet = "There are not any request yet!";
        public static string WorkflowUpdateMessage = "has update workflow";
        public static string CompletedRequestMessage = "Your request is completed";
        public static string ReceivedRequestMessage = "You received a request";
        public static string DeniedRequestMessage = "Your request is denied";
        public static string WrongCodeConfirm = "Wrong code! please try again";
        public static string UserNotExist = "User is not exist";
        public static string EmailExisted = "Email is existed!";
        public static string AccountCreated = "Account created, please check your email!";
        public static string EmptyList = "List is empty";
        public static string RequestIsHandled = "Sorry, this requested has been handled.";
        public static string WrongOldPassword = "Your current password was incorrect.";
        public static string PasswordToShort = "Passwords must be at least 6 characters.";
        public static string ToggleWorkflowFail = "Please design workflow first!";
    }
}
