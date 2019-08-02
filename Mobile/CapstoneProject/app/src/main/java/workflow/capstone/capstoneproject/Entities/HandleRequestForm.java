package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class HandleRequestForm {

    @SerializedName("initiatorName")
    @Expose
    private String initiatorName;

    @SerializedName("workFlowTemplateName")
    @Expose
    private String workFlowTemplateName;

    @SerializedName("workFlowTemplateActionName")
    @Expose
    private String workFlowTemplateActionName;

    @SerializedName("request")
    @Expose
    private Request request;

    @SerializedName("userRequestAction")
    @Expose
    private UserRequestAction userRequestAction;

    @SerializedName("staffRequestActions")
    @Expose
    private List<StaffRequestAction> staffRequestActions;

    @SerializedName("connections")
    @Expose
    private List<Connection> connections;

    @SerializedName("actionType")
    @Expose
    private ActionType actionType;

    public String getInitiatorName() {
        return initiatorName;
    }

    public void setInitiatorName(String initiatorName) {
        this.initiatorName = initiatorName;
    }

    public String getWorkFlowTemplateName() {
        return workFlowTemplateName;
    }

    public void setWorkFlowTemplateName(String workFlowTemplateName) {
        this.workFlowTemplateName = workFlowTemplateName;
    }

    public String getWorkFlowTemplateActionName() {
        return workFlowTemplateActionName;
    }

    public void setWorkFlowTemplateActionName(String workFlowTemplateActionName) {
        this.workFlowTemplateActionName = workFlowTemplateActionName;
    }

    public Request getRequest() {
        return request;
    }

    public void setRequest(Request request) {
        this.request = request;
    }

    public UserRequestAction getUserRequestAction() {
        return userRequestAction;
    }

    public void setUserRequestAction(UserRequestAction userRequestAction) {
        this.userRequestAction = userRequestAction;
    }

    public List<StaffRequestAction> getStaffRequestActions() {
        return staffRequestActions;
    }

    public void setStaffRequestActions(List<StaffRequestAction> staffRequestActions) {
        this.staffRequestActions = staffRequestActions;
    }

    public List<Connection> getConnections() {
        return connections;
    }

    public void setConnections(List<Connection> connections) {
        this.connections = connections;
    }

    public ActionType getActionType() {
        return actionType;
    }

    public void setActionType(ActionType actionType) {
        this.actionType = actionType;
    }
}
