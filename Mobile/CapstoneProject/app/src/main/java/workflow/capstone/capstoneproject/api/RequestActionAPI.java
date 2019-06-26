package workflow.capstone.capstoneproject.api;

public class RequestActionAPI {
    private int status;
    private String requestID;
    private String nextStepID;

    public RequestActionAPI(int status, String requestID, String nextStepID) {
        this.status = status;
        this.requestID = requestID;
        this.nextStepID = nextStepID;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
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
