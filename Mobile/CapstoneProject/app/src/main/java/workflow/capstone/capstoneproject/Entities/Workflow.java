package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Workflow {

    @SerializedName("workFlowID")
    @Expose
    private String workFlowID;
    @SerializedName("ownerID")
    @Expose
    private String ownerID;
    @SerializedName("name")
    @Expose
    private String name;
    @SerializedName("description")
    @Expose
    private String description;

    public String getWorkFlowID() {
        return workFlowID;
    }

    public void setWorkFlowID(String workFlowID) {
        this.workFlowID = workFlowID;
    }

    public String getOwnerID() {
        return ownerID;
    }

    public void setOwnerID(String ownerID) {
        this.ownerID = ownerID;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}
