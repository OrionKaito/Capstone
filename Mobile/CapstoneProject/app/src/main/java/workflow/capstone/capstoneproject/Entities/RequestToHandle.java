package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class RequestToHandle {

    @SerializedName("id")
    @Expose
    private String id;

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("initiatorID")
    @Expose
    private String initiatorID;

    @SerializedName("initiatorName")
    @Expose
    private String initiatorName;

    @SerializedName("workFlowTemplateID")
    @Expose
    private String workFlowTemplateID;

    @SerializedName("workFlowTemplateName")
    @Expose
    private String workFlowTemplateName;

    @SerializedName("createDate")
    @Expose
    private String createDate;

    @SerializedName("requestActionID")
    @Expose
    private String requestActionID;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getInitiatorID() {
        return initiatorID;
    }

    public void setInitiatorID(String initiatorID) {
        this.initiatorID = initiatorID;
    }

    public String getInitiatorName() {
        return initiatorName;
    }

    public void setInitiatorName(String initiatorName) {
        this.initiatorName = initiatorName;
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

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getRequestActionID() {
        return requestActionID;
    }

    public void setRequestActionID(String requestActionID) {
        this.requestActionID = requestActionID;
    }
}
