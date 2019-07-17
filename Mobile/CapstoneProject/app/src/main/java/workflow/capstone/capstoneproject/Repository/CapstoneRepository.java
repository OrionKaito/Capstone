package workflow.capstone.capstoneproject.repository;

import android.content.Context;

import java.util.List;
import java.util.Map;

import okhttp3.MultipartBody;
import okhttp3.ResponseBody;
import workflow.capstone.capstoneproject.api.Request;
import workflow.capstone.capstoneproject.api.RequestApprove;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;
import workflow.capstone.capstoneproject.entities.RequestForm;
import workflow.capstone.capstoneproject.entities.HandleRequestForm;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestResult;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.utils.CallBackData;

public interface CapstoneRepository {
    void login(Context context, Map<String, String> fields, CallBackData<Login> callBackData);

    void getProfile(String token, CallBackData<List<Profile>> callBackData);

    void updateProfile(Context context, String token, UpdateProfileModel model, CallBackData<String> callBackData);

    void changePassword(Context context, String token, String password, CallBackData<String> callBackData);

    void forgotPassword(Context context, String email, CallBackData<String> callBackData);

    void confirmForgotPassword(Context context, String code, String email, String newPassword, CallBackData<String> callBackData);

    void verifyAccount(Context context, String code, String email, CallBackData<String> callBackData);

    void getWorkflow(String token, CallBackData<List<WorkflowTemplate>> callBackData);

    void getNumberOfNotification(String token, int notificationType, CallBackData<String> callBackData);

    void getNotification(String token, CallBackData<List<UserNotification>> callBackData);

    void getNotificationByType(String token, int notificationType, CallBackData<List<UserNotification>> callBackData);

    void postRequest(String token, Request request, CallBackData<String> callBackData);

    void postRequestFile(String token, MultipartBody.Part file, CallBackData<String[]> callBackData);

    void postMultipleRequestFile(String token, MultipartBody.Part[] files, CallBackData<String[]> callBackData);

    void getRequestResult(String token, String requestActionID, String userNotificationID, CallBackData<RequestResult> callBackData);

    void getRequestForm(String token, String workflowTemplateID, CallBackData<RequestForm> callBackData);

    void getRequestHandleForm(String token, String requestActionID, CallBackData<HandleRequestForm> callBackData);

    void getAccountByUserID(String ID, CallBackData<List<Profile>> callBackData);

    void approveRequest(String token, RequestApprove requestApprove, CallBackData<String> callBackData);

    void downloadFileWithDynamicUrlSync(String fileUrl, CallBackData<ResponseBody> callBackData);
}
