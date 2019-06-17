package workflow.capstone.capstoneproject.api;

public class RequestActionAPI {
    private String status;
    private String requestID;
    private String nextStepID;

    public RequestActionAPI(String status, String requestID, String nextStepID) {
        this.status = status;
        this.requestID = requestID;
        this.nextStepID = nextStepID;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getRequestID() {
        return requestID;
    }

    public void setRequestID(String requestID) {
        this.requestID = requestID;
    }

    public String getNextStepID() {
        return nextStepID;
    }

    public void setNextStepID(String nextStepID) {
        this.nextStepID = nextStepID;
    }
}
