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

    @SerializedName("data")
    @Expose
    private String data;

    @SerializedName("permissionToUseID")
    @Expose
    private String permissionToUseID;

    @SerializedName("createDate")
    @Expose
    private String createDate;

    @SerializedName("isEnabled")
    @Expose
    private Boolean isEnabled;

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

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getPermissionToUseID() {
        return permissionToUseID;
    }

    public void setPermissionToUseID(String permissionToUseID) {
        this.permissionToUseID = permissionToUseID;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public Boolean getIsEnabled() {
        return isEnabled;
    }

    public void setIsEnabled(Boolean isEnabled) {
        this.isEnabled = isEnabled;
    }

}
