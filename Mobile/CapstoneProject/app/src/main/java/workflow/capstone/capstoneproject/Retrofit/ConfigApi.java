package workflow.capstone.capstoneproject.Retrofit;

public class ConfigApi {
    public static final String BASE_URL = "http://192.168.1.73:8080/api/";

    public interface Api {
        String LOGIN = "Token";
        String GET_PROFILE = "Accounts/GetProfile";
    }
}
