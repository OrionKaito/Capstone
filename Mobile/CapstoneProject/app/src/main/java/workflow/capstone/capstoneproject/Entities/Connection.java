package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Connection {

    @SerializedName("nextStepID")
    @Expose
    private String nextStepID;

    @SerializedName("connectionID")
    @Expose
    private String connectionID;

    @SerializedName("connectionTypeName")
    @Expose
    private String connectionTypeName;

    @SerializedName("nextStepTemplateActionID")
    @Expose
    private String nextStepTemplateActionID;

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }

    public String getConnectionID() {
        return connectionID;
    }

    public void setConnectionID(String connectionID) {
        this.connectionID = connectionID;
    }

    public String getConnectionTypeName() {
        return connectionTypeName;
    }

    public void setConnectionTypeName(String connectionTypeName) {
        this.connectionTypeName = connectionTypeName;
    }

    public String getNextStepTemplateActionID() {
        return nextStepTemplateActionID;
    }

    public void setNextStepTemplateActionID(String nextStepTemplateActionID) {
        this.nextStepTemplateActionID = nextStepTemplateActionID;
    }

}
