package workflow.capstone.capstoneproject.api;

import java.util.List;

public class Request {

    private String description;

    private String workFlowTemplateID;

    private String nextStepID;

    private Integer status;

    private List<ActionValue> actionValues = null;

    private List<String> imagePaths = null;

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
