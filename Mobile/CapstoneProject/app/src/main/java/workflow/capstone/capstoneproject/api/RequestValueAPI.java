package workflow.capstone.capstoneproject.api;

public class RequestValueAPI {
    private String data;
    private String requestActionID;

    public RequestValueAPI(String data, String requestActionID) {
        this.data = data;
        this.requestActionID = requestActionID;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getRequestActionID() {
        return requestActionID;
    }

    public void setRequestActionID(String requestActionID) {
        this.requestActionID = requestActionID;
    }
}
