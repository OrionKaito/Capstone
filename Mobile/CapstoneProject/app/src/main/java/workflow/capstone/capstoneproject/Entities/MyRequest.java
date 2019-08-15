package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class MyRequest {
    @SerializedName("id")
    @Expose
    private String id;

    @SerializedName("createDate")
    @Expose
    private String createDate;

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("workFlowTemplateID")
    @Expose
    private String workFlowTemplateID;

    @SerializedName("workFlowTemplateName")
    @Expose
    private String workFlowTemplateName;

    @SerializedName("currentRequestActionID")
    @Expose
    private String currentRequestActionID;

    @SerializedName("currentRequestActionName")
    @Expose
    private String currentRequestActionName;

    @SerializedName("isCompleted")
    @Expose
    private Boolean isCompleted;

    @SerializedName("isDeleted")
    @Expose
    private Boolean isDeleted;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getWorkFlowTemplateID() {
        return workFlowTemplateID;
    }

    public void setWorkFlowTemplateID(String workFlowTemplateID) {
        this.workFlowTemplateID = workFlowTemplateID;
    }

    public String getWorkFlowTemplateName() {
        return workFlowTemplateName;
    }

    public void setWorkFlowTemplateName(String workFlowTemplateName) {
        this.workFlowTemplateName = workFlowTemplateName;
    }

    public String getCurrentRequestActionID() {
        return currentRequestActionID;
    }

    public void setCurrentRequestActionID(String currentRequestActionID) {
        this.currentRequestActionID = currentRequestActionID;
    }

    public String getCurrentRequestActionName() {
        return currentRequestActionName;
    }

    public void setCurrentRequestActionName(String currentRequestActionName) {
        this.currentRequestActionName = currentRequestActionName;
    }

    public Boolean getIsCompleted() {
        return isCompleted;
    }

    public void setIsCompleted(Boolean isCompleted) {
        this.isCompleted = isCompleted;
    }

    public Boolean getIsDeleted() {
        return isDeleted;
    }

    public void setIsDeleted(Boolean isDeleted) {
        this.isDeleted = isDeleted;
    }
}
