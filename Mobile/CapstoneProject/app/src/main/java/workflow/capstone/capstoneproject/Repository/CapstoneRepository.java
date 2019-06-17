package workflow.capstone.capstoneproject.repository;

import android.content.Context;

import java.util.List;
import java.util.Map;

import workflow.capstone.capstoneproject.entities.DynamicForm.DynamicForm;
import workflow.capstone.capstoneproject.entities.Notification;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.RequestAction;
import workflow.capstone.capstoneproject.entities.WorkflowTemplate;
import workflow.capstone.capstoneproject.utils.CallBackData;

public interface CapstoneRepository {
    void login(Context context, Map<String, String> fields, CallBackData<String> callBackData);
    void getProfile(String token, CallBackData<Profile> callBackData);
    void getWorkflows(CallBackData<List<WorkflowTemplate>> callBackData);
    void getNumberOfNotification(String token, CallBackData<String> callBackData);
    void getNotification(String token, CallBackData<List<Notification>> callBackData);
    void postRequest(String description, String workFlowTemplateID, String token, CallBackData<String> callBackData);
    void postRequestAction(RequestAction requestAction, String token, CallBackData<String> callBackData);
    void postRequestValue(String data, String requestActionID, String token, CallBackData<String> callBackData);

    void getForm(CallBackData<DynamicForm> callBackData);
}
