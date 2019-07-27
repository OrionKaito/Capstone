export class AccountItem {

    constructor(roleID:any =[],
        groupIDs: any =[],
        LineManagerID,
        email,
        password,
        fullName,
        dateOfBirth){
            this.roleID= roleID ;
            this.groupIDs =groupIDs ;
            this.LineManagerID= LineManagerID;
            this.email= email;
            this.password=password;
            this.fullName = fullName;
            this.dateOfBirth = dateOfBirth;
        }
        id:any;
    roleID: any = [];
    groupIDs: any = [];
    LineManagerID: string;
    email: string;
    password: string;
    fullName: string;
    dateOfBirth: Date;
}
