package workflow.capstone.capstoneproject.retrofit;

import java.util.Map;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;

public interface DynamicWorkflowServices {
    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.LOGIN)
    Call<ResponseBody> login(@Body Map<String, String> fields);

    @GET(ConfigApi.Api.GET_PROFILE)
    Call<ResponseBody> getProfile();

    @GET(ConfigApi.Api.GET_WORKFLOWS)
    Call<ResponseBody> getWorkflows();
}
