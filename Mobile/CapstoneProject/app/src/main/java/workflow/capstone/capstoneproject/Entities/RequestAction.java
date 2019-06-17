package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class RequestAction implements Serializable {

    @SerializedName("status")
    @Expose
    private String status;
    @SerializedName("requestID")
    @Expose
    private String requestID;
    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getRequestID() {
        return requestID;
    }

    public void setRequestID(String requestID) {
        this.requestID = requestID;
    }

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }

}
