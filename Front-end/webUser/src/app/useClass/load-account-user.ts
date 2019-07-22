export class LoadAccountUser {
        id: string
        fullName: string
        email: string
        dateOfBirth: Date
        isDeleted: boolean
        role: string
        group: string
        constructor(id,fullName,email,dateOfBirth,isDeleted,role,group){
                this.id =id;
                this.fullName=fullName;
                this.email=email;
                this.dateOfBirth=dateOfBirth;
                this.isDeleted=isDeleted;
                this.role=role;
                this.group=group;
        }
}
