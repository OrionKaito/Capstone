package workflow.capstone.capstoneproject.entities;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class MyRequestPaging {

    @SerializedName("totalRecord")
    @Expose
    private Integer totalRecord;

    @SerializedName("myRequests")
    @Expose
    private List<MyRequest> myRequests;

    public Integer getTotalRecord() {
        return totalRecord;
    }

    public void setTotalRecord(Integer totalRecord) {
        this.totalRecord = totalRecord;
    }

    public List<MyRequest> getMyRequests() {
        return myRequests;
    }

    public void setMyRequests(List<MyRequest> myRequests) {
        this.myRequests = myRequests;
    }
}
