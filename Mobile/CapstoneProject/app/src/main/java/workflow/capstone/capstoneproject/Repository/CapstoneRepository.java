package workflow.capstone.capstoneproject.repository;

import android.content.Context;

import java.util.List;
import java.util.Map;

import workflow.capstone.capstoneproject.entities.Notification;
import workflow.capstone.capstoneproject.entities.Profile;
import workflow.capstone.capstoneproject.entities.Workflow;
import workflow.capstone.capstoneproject.utils.CallBackData;

public interface CapstoneRepository {
    void login(Context context, Map<String, String> fields, CallBackData<String> callBackData);
    void getProfile(String token, CallBackData<Profile> callBackData);
    void getWorkflows(String token, CallBackData<List<Workflow>> callBackData);
    void getNumberOfNotification(String token, CallBackData<String> callBackData);
    void getNotification(String token, CallBackData<List<Notification>> callBackData);
}
