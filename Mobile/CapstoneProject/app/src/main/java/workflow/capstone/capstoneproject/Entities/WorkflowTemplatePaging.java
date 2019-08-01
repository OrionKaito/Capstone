package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class WorkflowTemplatePaging {
    @SerializedName("totalRecord")
    @Expose
    private Integer totalRecord;

    @SerializedName("workFlowTemplates")
    @Expose
    private List<WorkflowTemplate> workFlowTemplates;

    public Integer getTotalRecord() {
        return totalRecord;
    }

    public void setTotalRecord(Integer totalRecord) {
        this.totalRecord = totalRecord;
    }

    public List<WorkflowTemplate> getWorkFlowTemplates() {
        return workFlowTemplates;
    }

    public void setWorkFlowTemplates(List<WorkflowTemplate> workFlowTemplates) {
        this.workFlowTemplates = workFlowTemplates;
    }
}
