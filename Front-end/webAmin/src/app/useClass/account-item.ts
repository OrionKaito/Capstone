export class AccountItem {

    constructor(roleIDs:any =[],
        groupIDs: any =[],
        managerID,
        email,
        password,
        fullName,
        dateOfBirth){
            this.roleIDs= roleIDs ;
            this.groupIDs =groupIDs ;
            this.managerID= managerID;
            this.email= email;
            this.password=password;
            this.fullName = fullName;
            this.dateOfBirth = dateOfBirth;
        }
        id:any;
    roleIDs: any = [];
    groupIDs: any = [];
    managerID: string;
    email: string;
    password: string;
    fullName: string;
    dateOfBirth: Date;
}
