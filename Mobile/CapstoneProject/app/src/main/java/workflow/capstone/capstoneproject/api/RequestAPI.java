package workflow.capstone.capstoneproject.api;

public class RequestAPI {
    private String description;
    private String workFlowTemplateID;

    public RequestAPI(String description, String workFlowTemplateID) {
        this.description = description;
        this.workFlowTemplateID = workFlowTemplateID;
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
}
