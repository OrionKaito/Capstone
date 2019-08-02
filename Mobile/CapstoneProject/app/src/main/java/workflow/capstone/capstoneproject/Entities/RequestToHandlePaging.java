package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class RequestToHandlePaging {

    @SerializedName("totalRecord")
    @Expose
    private Integer totalRecord;

    @SerializedName("requests")
    @Expose
    private List<RequestToHandle> requests;

    public Integer getTotalRecord() {
        return totalRecord;
    }

    public void setTotalRecord(Integer totalRecord) {
        this.totalRecord = totalRecord;
    }

    public List<RequestToHandle> getRequests() {
        return requests;
    }

    public void setRequests(List<RequestToHandle> requests) {
        this.requests = requests;
    }
}
