package workflow.capstone.capstoneproject.retrofit;

import java.util.Map;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import workflow.capstone.capstoneproject.api.RequestAPI;
import workflow.capstone.capstoneproject.api.RequestActionAPI;
import workflow.capstone.capstoneproject.api.RequestValueAPI;

public interface DynamicWorkflowServices {
    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.LOGIN)
    Call<ResponseBody> login(@Body Map<String, String> fields);

    @GET(ConfigApi.Api.GET_PROFILE)
    Call<ResponseBody> getProfile();

    @GET(ConfigApi.Api.GET_WORKFLOWS)
    Call<ResponseBody> getWorkflows();

    @GET(ConfigApi.Api.GET_NUMBER_NOTIFICATION)
    Call<ResponseBody> getNumberNotification();

    @GET(ConfigApi.Api.GET_NOTIFCATION)
    Call<ResponseBody> getNotification();

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.POST_REQUEST)
    Call<ResponseBody> postRequest(@Body RequestAPI requestAPI);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.POST_REQUEST_ACTION)
    Call<ResponseBody> postRequestAction(@Body RequestActionAPI requestActionAPI);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.POST_REQUEST_VALUE)
    Call<ResponseBody> postRequestValue(@Body RequestValueAPI requestValueAPI);

    @GET("https://demo6278327.mockable.io/GetForm")
    Call<ResponseBody> getForm();
}
