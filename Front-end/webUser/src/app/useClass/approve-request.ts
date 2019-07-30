export class ApproveRequest {
    constructor(requestID, nextStepID, actionValues, requestActionID){
        this.requestID =requestID;
        this.nextStepID = nextStepID;
        this.actionValues= actionValues;
        this.requestActionID = requestActionID;

    }
    requestID: string;
    requestActionID: string;
    status=1;
    nextStepID: string;
    actionValues: any = [];
}
