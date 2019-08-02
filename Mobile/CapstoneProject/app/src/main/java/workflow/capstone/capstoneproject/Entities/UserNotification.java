package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.io.Serializable;

public class UserNotification implements Serializable {

    @SerializedName("workflowName")
    @Expose
    private String workflowName;

    @SerializedName("userNotificationID")
    @Expose
    private String userNotificationID;

    @SerializedName("eventID")
    @Expose
    private String eventID;

    @SerializedName("message")
    @Expose
    private String message;

    @SerializedName("actorName")
    @Expose
    private String actorName;

    @SerializedName("notificationType")
    @Expose
    private Integer notificationType;

    @SerializedName("notificationTypeName")
    @Expose
    private String notificationTypeName;

    @SerializedName("createDate")
    @Expose
    private String createDate;

    @SerializedName("isRead")
    @Expose
    private Boolean isRead;

    @SerializedName("isHandled")
    @Expose
    private Boolean isHandled;

    public String getWorkflowName() {
        return workflowName;
    }

    public void setWorkflowName(String workflowName) {
        this.workflowName = workflowName;
    }

    public String getUserNotificationID() {
        return userNotificationID;
    }

    public void setUserNotificationID(String userNotificationID) {
        this.userNotificationID = userNotificationID;
    }

    public String getEventID() {
        return eventID;
    }

    public void setEventID(String eventID) {
        this.eventID = eventID;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getActorName() {
        return actorName;
    }

    public void setActorName(String actorName) {
        this.actorName = actorName;
    }

    public Integer getNotificationType() {
        return notificationType;
    }

    public void setNotificationType(Integer notificationType) {
        this.notificationType = notificationType;
    }

    public String getNotificationTypeName() {
        return notificationTypeName;
    }

    public void setNotificationTypeName(String notificationTypeName) {
        this.notificationTypeName = notificationTypeName;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public Boolean getIsRead() {
        return isRead;
    }

    public void setIsRead(Boolean isRead) {
        this.isRead = isRead;
    }

    public Boolean getIsHandled() {
        return isHandled;
    }

    public void setIsHandled(Boolean isHandled) {
        this.isHandled = isHandled;
    }
}