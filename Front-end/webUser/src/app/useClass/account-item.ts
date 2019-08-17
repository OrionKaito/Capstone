export class AccountItem {
    constructor(){
        this.id ="";
        this.name="";
        this.description="";
        this.data="";
        this.permissionToEditID="";
        this.permissionToUseID="";
        this.icon="folder_special";
        this.isViewDetail=true;
    }

    id: string;
    name: string;
    description: string;
    data: string;
    permissionToEditID: string;
    permissionToUseID: string;
    icon: string;
    isViewDetail: boolean;

}
