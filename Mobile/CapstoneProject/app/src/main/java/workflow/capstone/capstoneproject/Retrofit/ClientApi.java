package workflow.capstone.capstoneproject.retrofit;

public class ClientApi extends BaseApi{
    public DynamicWorkflowServices getDynamicWorkflowServices() {
        return this.getService(DynamicWorkflowServices.class, ConfigApi.BASE_URL);
    }

    public DynamicWorkflowServices getDWServices(String token) {
        return this.getServiceWithAuthorization(DynamicWorkflowServices.class, ConfigApi.BASE_URL, token);
    }
}
