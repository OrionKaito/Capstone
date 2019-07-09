export class SendRequest {
    constructor(description, actionValues, imagePaths, workFlowTemplateID, nextStepID){
        this.description =description;
        this.workFlowTemplateID = workFlowTemplateID;
        this.nextStepID = nextStepID;
        this.actionValues= actionValues;
        this.imagePaths = imagePaths;
    }
    description: string
    workFlowTemplateID: string
    nextStepID: string
    status: 1
    actionValues: any = []
    imagePaths: any = []
}
