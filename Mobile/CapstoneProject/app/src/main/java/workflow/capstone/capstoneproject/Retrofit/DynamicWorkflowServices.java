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
import workflow.capstone.capstoneproject.api.RequestModel;
import workflow.capstone.capstoneproject.api.RequestApproveModel;
import workflow.capstone.capstoneproject.api.LoginModel;
import workflow.capstone.capstoneproject.api.UpdateAvatarModel;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;

public interface DynamicWorkflowServices {
    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.LOGIN)
    Call<ResponseBody> login(@Body Map<String, String> fields);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.NEW_LOGIN)
    Call<ResponseBody> newLogin(@Body LoginModel loginModel);

    @PUT(ConfigApi.Api.LOGOUT)
    Call<ResponseBody> logout(@Query(value = "deviceToken", encoded = true) String deviceToken);

    @GET(ConfigApi.Api.GET_PROFILE)
    Call<ResponseBody> getProfile();

    @Headers({"Content-Type:application/json"})
    @PUT(ConfigApi.Api.UPDATE_PROFILE)
    Call<ResponseBody> updateProfile(@Body UpdateProfileModel model);

    @PUT(ConfigApi.Api.UPDATE_AVATAR)
    Call<ResponseBody> updateAvatar(@Body UpdateAvatarModel updateAvatarModel);

    @PUT(ConfigApi.Api.CHANGE_PASSWORD)
    Call<ResponseBody> changePassword(@Query(value = "oldPassword", encoded = true) String oldPassword, @Query(value = "newPassword", encoded = true) String newPassword);

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
    Call<ResponseBody> getWorkflow(@Query(value = "numberOfPage", encoded = true) Integer numberOfPage, @Query(value = "NumberOfRecord", encoded = true) Integer NumberOfRecord);

    @GET(ConfigApi.Api.GET_NUMBER_NOTIFICATION)
    Call<ResponseBody> getNumberNotification();

    @GET(ConfigApi.Api.GET_NOTIFICATION)
    Call<ResponseBody> getNotification(@Query(value = "numberOfPage", encoded = true) Integer numberOfPage, @Query(value = "NumberOfRecord", encoded = true) Integer NumberOfRecord);

    @GET(ConfigApi.Api.GET_REQUESTS_TO_HANDLE_BY_PERMISSION)
    Call<ResponseBody> getRequestsToHandleByPermission(@Query(value = "numberOfPage", encoded = true) Integer numberOfPage, @Query(value = "NumberOfRecord", encoded = true) Integer NumberOfRecord);

    @GET(ConfigApi.Api.GET_NOTIFICATION_BY_TYPE)
    Call<ResponseBody> getNotificationByType(@Query(value = "notificationType", encoded = true) int notificationType);

    @GET(ConfigApi.Api.GET_MY_REQUEST)
    Call<ResponseBody> getMyRequest(@Query(value = "numberOfPage", encoded = true) Integer numberOfPage, @Query(value = "NumberOfRecord", encoded = true) Integer NumberOfRecord);

    @Headers({"Content-Type:application/json"})
    @POST(ConfigApi.Api.POST_REQUEST)
    Call<ResponseBody> postRequest(@Body RequestModel requestModel);

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
    Call<ResponseBody> approveRequest(@Body RequestApproveModel requestApproveModel);

}
