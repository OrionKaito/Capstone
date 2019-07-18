package workflow.capstone.capstoneproject.retrofit;

import java.util.Map;

import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Part;
import retrofit2.http.Query;
import retrofit2.http.Url;
import workflow.capstone.capstoneproject.api.Request;
import workflow.capstone.capstoneproject.api.RequestApprove;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;

public interface DynamicWorkflowServices {
    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.LOGIN)
    Call<ResponseBody> login(@Body Map<String, String> fields);

    @GET(ConfigApi.Api.GET_PROFILE)
    Call<ResponseBody> getProfile();

    @Headers({"Content-Type:application/json"})
    @PUT(ConfigApi.Api.UPDATE_PROFILE)
    Call<ResponseBody> updateProfile(@Body UpdateProfileModel model);

    @PUT(ConfigApi.Api.CHANGE_PASSWORD)
    Call<ResponseBody> changePassword(@Query(value = "password", encoded = true) String password);

    @POST(ConfigApi.Api.FORGOT_PASSWORD)
    Call<ResponseBody> forgotPassword(@Query(value = "email", encoded = true) String email);

    @PUT(ConfigApi.Api.CONFIRM_FORGOT_PASSWORD)
    Call<ResponseBody> confirmForgotPassword(@Query(value = "code", encoded = true) String code,
                                             @Query(value = "email", encoded = true) String email,
                                             @Query(value = "password", encoded = true) String password);

    @POST(ConfigApi.Api.VERIFY_ACCOUNT)
    Call<ResponseBody> verifyAccount(@Query(value = "code", encoded = true) String code,
                                     @Query(value = "email", encoded = true) String email);

    @GET(ConfigApi.Api.GET_WORKFLOW)
    Call<ResponseBody> getWorkflow();

    @GET(ConfigApi.Api.GET_NUMBER_NOTIFICATION)
    Call<ResponseBody> getNumberNotification(@Query(value = "notificationType", encoded = true) int notificationType);

    @GET(ConfigApi.Api.GET_NOTIFICATION)
    Call<ResponseBody> getNotification();

    @GET(ConfigApi.Api.GET_NOTIFICATION_BY_TYPE)
    Call<ResponseBody> getNotificationByType(@Query(value = "notificationType", encoded = true) int notificationType);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.POST_REQUEST)
    Call<ResponseBody> postRequest(@Body Request request);

    @Multipart
    @POST(ConfigApi.Api.POST_REQUEST_FILE)
    Call<ResponseBody> upFile(
            @Part MultipartBody.Part file
    );

    @Multipart
    @POST(ConfigApi.Api.POST_REQUEST_FILE)
    Call<ResponseBody> uploadMultipleFile(
            @Part MultipartBody.Part[] files
    );

    @GET(ConfigApi.Api.GET_REQUEST_RESULT)
    Call<ResponseBody> getRequestResult(@Query(value = "requestActionID", encoded = true) String requestActionID, @Query(value = "userNotificationID", encoded = true) String userNotificationID);

    @GET(ConfigApi.Api.GET_REQUEST_FORM)
    Call<ResponseBody> getRequestForm(@Query(value = "workFlowTemplateID", encoded = true) String workFlowTemplateID);

    @GET(ConfigApi.Api.GET_REQUEST_HANDLE_FORM)
    Call<ResponseBody> getRequestHandleForm(@Query(value = "requestActionID", encoded = true) String requestActionID);

    @GET(ConfigApi.Api.GET_ACCOUNT_BY_USER_ID)
    Call<ResponseBody> getAccountByUserID(@Query(value = "ID", encoded = true) String ID);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.APPROVE_REQUEST)
    Call<ResponseBody> approveRequest(@Body RequestApprove requestApprove);

    @GET
    Call<ResponseBody> downloadFileWithDynamicUrlSync(@Url String fileUrl);

}
