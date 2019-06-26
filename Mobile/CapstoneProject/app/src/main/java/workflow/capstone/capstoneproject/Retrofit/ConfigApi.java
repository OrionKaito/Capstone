package workflow.capstone.capstoneproject.retrofit;

public class ConfigApi {
    public static final String BASE_URL = "http://192.168.1.24:159/api/";

    public interface Api {
        String LOGIN = "Token/User";
        String GET_PROFILE = "Accounts/GetProfile";
        String GET_WORKFLOW = "WorkflowsTemplates";
        String GET_NUMBER_NOTIFICATION = "Notifications/GetNumberOfNotification";
        String GET_NOTIFICATION = "Notifications/GetByUserID";
        String POST_REQUEST = "Requests";
        String POST_REQUEST_FILE = "RequestFiles";
    }
}
