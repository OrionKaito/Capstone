package workflow.capstone.capstoneproject.retrofit;

public class ConfigApi {
    public static final String BASE_URL = "http://192.168.1.23:8080/api/";

    public interface Api {
        String LOGIN = "Token/User";
        String GET_PROFILE = "Accounts/GetProfile";
        String GET_WORKFLOWS = "Workflows";
        String GET_NUMBER_NOTIFICATION = "Notifications/GetNumberOfNotification";
        String GET_NOTIFCATION = "Notifications/GetByUserID";
    }
}
