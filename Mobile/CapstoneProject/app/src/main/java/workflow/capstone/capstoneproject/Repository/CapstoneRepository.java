package workflow.capstone.capstoneproject.Repository;

import java.util.Map;

import workflow.capstone.capstoneproject.Entities.Profile;
import workflow.capstone.capstoneproject.utils.CallBackData;

public interface CapstoneRepository {
    void login(Map<String, String> fields, CallBackData<String> callBackData);
    void getProfile(String token, CallBackData<Profile> callBackData);
}
