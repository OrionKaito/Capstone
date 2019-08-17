package workflow.capstone.capstoneproject.retrofit;

public class ConfigApi {
    public static final String BASE_URL = "http://192.168.1.85:1234/api/";

    public interface Api {
        String LOGIN = "Token/User";
        String NEW_LOGIN = "Token/NewLogin";
        String LOGOUT = "Token/Logout";
        String GET_PROFILE = "Accounts/GetProfile";
        String UPDATE_PROFILE = "Accounts/UpdateProfile";
        String UPDATE_AVATAR = "Accounts/UpdateAvatar";
        String CHANGE_PASSWORD = "Accounts/ChangePassword";
        String FORGOT_PASSWORD = "Accounts/ForgotPassword";
        String CONFIRM_FORGOT_PASSWORD = "Accounts/ConfirmForgotPassword";
        String VERIFY_ACCOUNT = "Accounts/ConfirmEmail";
        String GET_WORKFLOW = "WorkflowsTemplates/GetWorkflowToUse";
        String GET_NUMBER_NOTIFICATION = "UserNotifications/GetNumberNotification";
        String GET_NOTIFICATION = "UserNotifications/GetNotificationByUser";
        String GET_REQUESTS_TO_HANDLE_BY_PERMISSION = "Requests/GetRequestsToHandleByPermission";
        String GET_NOTIFICATION_BY_TYPE = "UserNotifications/GetNotificationByUserId";
        String GET_MY_REQUEST = "Requests/GetMyRequests";
        String POST_REQUEST = "Requests";
        String POST_REQUEST_FILE = "RequestFiles";
        String GET_REQUEST_RESULT = "Requests/GetRequestResult";
        String GET_REQUEST_FORM = "Requests/GetRequestForm";
        String GET_REQUEST_HANDLE_FORM = "Requests/GetRequestHandleForm";
        String APPROVE_REQUEST = "Requests/ApproveRequest";
        String GET_ACCOUNT_BY_USER_ID = "Accounts/GetAccountByUserID";
    }
}
