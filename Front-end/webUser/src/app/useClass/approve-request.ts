export class ApproveRequest {
    constructor(requestID, nextStepID, actionValues){
        this.requestID =requestID;
        this.nextStepID = nextStepID;
        this.actionValues= actionValues;

    }
    requestID: string;
    status=1;
    nextStepID: string;
    actionValues: any = [];
}
