package workflow.capstone.capstoneproject.entities;

import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class StaffRequestAction {

    @SerializedName("fullName")
    @Expose
    private String fullName;

    @SerializedName("userName")
    @Expose
    private String userName;

    @SerializedName("createDate")
    @Expose
    private String createDate;

    @SerializedName("status")
    @Expose
    private String status;

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("requestValues")
    @Expose
    private List<RequestValue> requestValues;

    @SerializedName("workFlowActionName")
    @Expose
    private String workFlowActionName;

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public List<RequestValue> getRequestValues() {
        return requestValues;
    }

    public void setRequestValues(List<RequestValue> requestValues) {
        this.requestValues = requestValues;
    }

    public String getWorkFlowActionName() {
        return workFlowActionName;
    }

    public void setWorkFlowActionName(String workFlowActionName) {
        this.workFlowActionName = workFlowActionName;
    }

}