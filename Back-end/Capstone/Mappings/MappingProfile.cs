using AutoMapper;
using Capstone.Model;
using Capstone.ViewModel;

namespace Capstone.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegistrationVM>();
            CreateMap<RegistrationVM, User>();

            CreateMap<WorkFlow, WorkflowVM>();
            CreateMap<WorkflowVM, WorkFlow>();

            CreateMap<WorkFlow, WorkflowCM>();
            CreateMap<WorkflowCM, WorkFlow>();

            //ActionType
            CreateMap<ActionType, ActionTypeVM>();
            CreateMap<ActionTypeVM, ActionType>();

            CreateMap<ActionType, ActionTypeCM>();
            CreateMap<ActionTypeCM, ActionType>();

            CreateMap<ActionType, ActionTypeUM>();
            CreateMap<ActionTypeUM, ActionType>();

            //Group
            CreateMap<Group, GroupVM>();
            CreateMap<GroupVM, Group>();

            CreateMap<Group, GroupCM>();
            CreateMap<GroupCM, Group>();

            CreateMap<Group, GroupUM>();
            CreateMap<GroupUM, Group>();

            //UserGroup
            CreateMap<UserGroup, UserGroupVM>();
            CreateMap<UserGroupVM, UserGroup>();

            CreateMap<UserGroup, UserGroupCM>();
            CreateMap<UserGroupCM, UserGroup>();

            CreateMap<UserGroup, UserGroupUM>();
            CreateMap<UserGroupUM, UserGroup>();

            //Role
            CreateMap<Role, RoleVM>();
            CreateMap<RoleVM, Role>();

            CreateMap<Role, RoleCM>();
            CreateMap<RoleCM, Role>();

            CreateMap<Role, RoleUM>();
            CreateMap<RoleUM, Role>();

            //RoleOfGroup
            CreateMap<RoleOfGroup, RoleOfGroupVM>();
            CreateMap<RoleOfGroupVM, RoleOfGroup>();

            CreateMap<RoleOfGroup, RoleOfGroupCM>();
            CreateMap<RoleOfGroupCM, RoleOfGroup>();

            CreateMap<RoleOfGroup, RoleOfGroupUM>();
            CreateMap<RoleOfGroupUM, RoleOfGroup>();

            //Permission
            CreateMap<Permission, PermissionVM>();
            CreateMap<PermissionVM, Permission>();

            CreateMap<Permission, PermissionCM>();
            CreateMap<PermissionCM, Permission>();

            CreateMap<Permission, PermissionUM>();
            CreateMap<PermissionUM, Permission>();

            //PermissionOfRole
            CreateMap<PermissionOfRole, PermissionOfRoleVM>();
            CreateMap<PermissionOfRoleVM, PermissionOfRole>();

            CreateMap<PermissionOfRole, PermissionOfRoleCM>();
            CreateMap<PermissionOfRoleCM, PermissionOfRole>();

            CreateMap<PermissionOfRole, PermissionOfRoleUM>();
            CreateMap<PermissionOfRoleUM, PermissionOfRole>();
        }
    }
}
