package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class StaffResult {

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

    @SerializedName("requestValues")
    @Expose
    private List<RequestValue> requestValues;

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

    public List<RequestValue> getRequestValues() {
        return requestValues;
    }

    public void setRequestValues(List<RequestValue> requestValues) {
        this.requestValues = requestValues;
    }

}
