package workflow.capstone.capstoneproject.api;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class Request {

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("workFlowTemplateID")
    @Expose
    private String workFlowTemplateID;

    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    @SerializedName("status")
    @Expose
    private Integer status;

    @SerializedName("myProperty")
    @Expose
    private Integer myProperty;

    @SerializedName("actionValues")
    @Expose
    private List<ActionValue> actionValues;

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

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public Integer getMyProperty() {
        return myProperty;
    }

    public void setMyProperty(Integer myProperty) {
        this.myProperty = myProperty;
    }

    public List<ActionValue> getActionValues() {
        return actionValues;
    }

    public void setActionValues(List<ActionValue> actionValues) {
        this.actionValues = actionValues;
    }

    public List<String> getImagePaths() {
        return imagePaths;
    }

    public void setImagePaths(List<String> imagePaths) {
        this.imagePaths = imagePaths;
    }
}
