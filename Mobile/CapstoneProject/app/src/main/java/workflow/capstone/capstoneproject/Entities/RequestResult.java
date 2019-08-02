package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RequestResult {

    @SerializedName("workFlowTemplateName")
    @Expose
    private String workFlowTemplateName;

    @SerializedName("status")
    @Expose
    private String status;

    @SerializedName("staffResult")
    @Expose
    private List<StaffResult> staffResult;

    public String getWorkFlowTemplateName() {
        return workFlowTemplateName;
    }

    public void setWorkFlowTemplateName(String workFlowTemplateName) {
        this.workFlowTemplateName = workFlowTemplateName;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public List<StaffResult> getStaffResult() {
        return staffResult;
    }

    public void setStaffResult(List<StaffResult> staffResult) {
        this.staffResult = staffResult;
    }
}
