package workflow.capstone.capstoneproject.api;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RequestApprove {

    @SerializedName("requestID")
    @Expose
    private String requestID;

    @SerializedName("requestActionID")
    @Expose
    private String requestActionID;

    @SerializedName("status")
    @Expose
    private Integer status;

    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    @SerializedName("actionValues")
    @Expose
    private List<ActionValue> actionValues = null;

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

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }

    public List<ActionValue> getActionValues() {
        return actionValues;
    }

    public void setActionValues(List<ActionValue> actionValues) {
        this.actionValues = actionValues;
    }
}
