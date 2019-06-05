package workflow.capstone.capstoneproject.Retrofit;

public class ClientApi extends BaseApi{
    public DynamicWorkflowServices getDynamicWorkflowServices() {
        return this.getService(DynamicWorkflowServices.class, ConfigApi.BASE_URL);
    }
}
