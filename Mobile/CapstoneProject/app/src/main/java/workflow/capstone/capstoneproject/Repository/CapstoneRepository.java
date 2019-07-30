package workflow.capstone.capstoneproject.repository;

import android.content.Context;

import java.util.List;
import java.util.Map;

import okhttp3.MultipartBody;
import workflow.capstone.capstoneproject.api.LoginModel;
import workflow.capstone.capstoneproject.api.RequestApproveModel;
import workflow.capstone.capstoneproject.api.RequestModel;
import workflow.capstone.capstoneproject.api.UpdateAvatarModel;
import workflow.capstone.capstoneproject.api.UpdateProfileModel;
import workflow.capstone.capstoneproject.entities.HandleRequestForm;
import workflow.capstone.capstoneproject.entities.Login;
import workflow.capstone.capstoneproject.entities.MyRequest;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestForm;
import workflow.capstone.capstoneproject.entities.RequestResult;
import workflow.capstone.capstoneproject.entities.RequestToHandle;
import workflow.capstone.capstoneproject.entities.UserNotification;
import workflow.capstone.capstoneproject.entities.WorkflowTemplatePaging;
import workflow.capstone.capstoneproject.utils.CallBackData;

public interface CapstoneRepository {
    void login(Context context, Map<String, String> fields, CallBackData<Login> callBackData);

    void newLogin(Context context, LoginModel loginModel, CallBackData<Login> callBackData);

    void logout(String token, String deviceToken, CallBackData<String> callBackData);

    void getProfile(String token, CallBackData<List<Profile>> callBackData);

    void updateProfile(Context context, String token, UpdateProfileModel model, CallBackData<String> callBackData);

    void updateAvatar(Context context, String token, UpdateAvatarModel updateAvatarModel, CallBackData<String> callBackData);

    void changePassword(Context context, String token, String oldPassword, String newPassword, CallBackData<String> callBackData);

    void forgotPassword(Context context, String email, CallBackData<String> callBackData);

    void confirmForgotPassword(Context context, String code, String email, String newPassword, CallBackData<String> callBackData);

    void verifyAccount(Context context, String code, String email, CallBackData<String> callBackData);

    void getWorkflow(String token, int numberOfPage, int numberOfRecord, CallBackData<WorkflowTemplatePaging> callBackData);

    void getNumberOfNotification(String token, CallBackData<String> callBackData);

    void getNotification(String token, CallBackData<List<UserNotification>> callBackData);

    void getRequestsToHandleByPermission(String token, CallBackData<List<RequestToHandle>> callBackData);

    void getNotificationByType(String token, int notificationType, CallBackData<List<UserNotification>> callBackData);

    void getMyRequest(String token, CallBackData<List<MyRequest>> callBackData);

    void postRequest(String token, RequestModel requestModel, CallBackData<String> callBackData);

    void postRequestFile(String token, MultipartBody.Part file, CallBackData<String[]> callBackData);

    void postMultipleRequestFile(String token, MultipartBody.Part[] files, CallBackData<String[]> callBackData);

    void getRequestResult(String token, String requestActionID, String userNotificationID, CallBackData<RequestResult> callBackData);

    void getRequestForm(String token, String workflowTemplateID, CallBackData<RequestForm> callBackData);

    void getRequestHandleForm(String token, String requestActionID, CallBackData<HandleRequestForm> callBackData);

    void getAccountByUserID(String ID, CallBackData<List<Profile>> callBackData);

    void approveRequest(String token, RequestApproveModel requestApproveModel, CallBackData<String> callBackData);

}
