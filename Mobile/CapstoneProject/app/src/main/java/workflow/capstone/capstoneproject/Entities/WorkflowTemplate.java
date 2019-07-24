package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class WorkflowTemplate implements Serializable {
    @SerializedName("id")
    @Expose
    private String id;

    @SerializedName("ownerID")
    @Expose
    private String ownerID;

    @SerializedName("name")
    @Expose
    private String name;

    @SerializedName("description")
    @Expose
    private String description;

    @SerializedName("permissionToEditID")
    @Expose
    private String permissionToEditID;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
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

    public String getPermissionToEditID() {
        return permissionToEditID;
    }

    public void setPermissionToEditID(String permissionToEditID) {
        this.permissionToEditID = permissionToEditID;
    }
}
