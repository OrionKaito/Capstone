package workflow.capstone.capstoneproject.api;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RequestApproveModel {

    @SerializedName("requestID")
    @Expose
    private String requestID;

    @SerializedName("requestActionID")
    @Expose
    private String requestActionID;

    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    @SerializedName("actionValues")
    @Expose
    private List<ActionValueModel> actionValueModels = null;

    public String getRequestID() {
        return requestID;
    }

    public void setRequestID(String requestID) {
        this.requestID = requestID;
    }

    public String getRequestActionID() {
        return requestActionID;
    }

    public void setRequestActionID(String requestActionID) {
        this.requestActionID = requestActionID;
    }

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }

    public List<ActionValueModel> getActionValueModels() {
        return actionValueModels;
    }

    public void setActionValueModels(List<ActionValueModel> actionValueModels) {
        this.actionValueModels = actionValueModels;
    }
}
