package workflow.capstone.capstoneproject.api;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RequestModel {

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("workFlowTemplateID")
    @Expose
    private String workFlowTemplateID;

    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    @SerializedName("workFlowTemplateActionID")
    @Expose
    private String workFlowTemplateActionID;

    @SerializedName("actionValues")
    @Expose
    private List<ActionValueModel> actionValues;

    @SerializedName("imagePaths")
    @Expose
    private List<String> imagePaths;

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

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }

    public String getWorkFlowTemplateActionID() {
        return workFlowTemplateActionID;
    }

    public void setWorkFlowTemplateActionID(String workFlowTemplateActionID) {
        this.workFlowTemplateActionID = workFlowTemplateActionID;
    }

    public List<ActionValueModel> getActionValues() {
        return actionValues;
    }

    public void setActionValues(List<ActionValueModel> actionValues) {
        this.actionValues = actionValues;
    }

    public List<String> getImagePaths() {
        return imagePaths;
    }

    public void setImagePaths(List<String> imagePaths) {
        this.imagePaths = imagePaths;
    }

}
