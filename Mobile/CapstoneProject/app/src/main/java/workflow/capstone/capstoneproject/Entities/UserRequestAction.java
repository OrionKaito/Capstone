package workflow.capstone.capstoneproject.entities;

import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UserRequestAction {

    @SerializedName("requestFiles")
    @Expose
    private List<RequestFile> requestFiles;

    @SerializedName("requestValues")
    @Expose
    private List<RequestValue> requestValues;

    public List<RequestFile> getRequestFiles() {
        return requestFiles;
    }

    public void setRequestFiles(List<RequestFile> requestFiles) {
        this.requestFiles = requestFiles;
    }

    public List<RequestValue> getRequestValues() {
        return requestValues;
    }

    public void setRequestValues(List<RequestValue> requestValues) {
        this.requestValues = requestValues;
    }

}