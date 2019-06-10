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
            
            CreateMap<User, RegistrationUM>();
            CreateMap<RegistrationUM, User>();

            CreateMap<WorkFlowTemplate, WorkFlowTemplateVM>();
            CreateMap<WorkFlowTemplateVM, WorkFlowTemplate>();

            CreateMap<WorkFlowTemplate, WorkFlowTemplateCM>();
            CreateMap<WorkFlowTemplateCM, WorkFlowTemplate>();

            CreateMap<WorkFlowTemplate, WorkFlowTemplateUM>();
            CreateMap<WorkFlowTemplateUM, WorkFlowTemplate>();

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

            //UserRole
            CreateMap<UserRole, UserRoleVM>();
            CreateMap<UserRoleVM, UserRole>();

            CreateMap<UserRole, UserRoleCM>();
            CreateMap<UserRoleCM, UserRole>();

            CreateMap<UserGroup, UserRoleUM>();
            CreateMap<UserRoleUM, UserRole>();

            //Request
            CreateMap<Request, RequestVM>();
            CreateMap<RequestVM, Request>();

            CreateMap<Request, RequestCM>();
            CreateMap<RequestCM, Request>();

            CreateMap<Request, RequestUM>();
            CreateMap<RequestUM, Request>();

            //RequestAction
            CreateMap<RequestAction, RequestActionVM>();
            CreateMap<RequestActionVM, RequestAction>();

            CreateMap<RequestAction, RequestActionCM>();
            CreateMap<RequestActionCM, RequestAction>();

            CreateMap<RequestAction, RequestActionUM>();
            CreateMap<RequestActionUM, RequestAction>();

            //RequestValue
            CreateMap<RequestValue, RequestValueVM>();
            CreateMap<RequestValueVM, RequestValue>();

            CreateMap<RequestValue, RequestValueCM>();
            CreateMap<RequestValueCM, RequestValue>();

            CreateMap<RequestValue, RequestValueUM>();
            CreateMap<RequestValueUM, RequestValue>();

            //RequestFile
            CreateMap<RequestFile, RequestFileVM>();
            CreateMap<RequestFileVM, RequestFile>();

            CreateMap<RequestFile, RequestFileCM>();
            CreateMap<RequestFileCM, RequestFile>();
            
            CreateMap<RequestFile, RequestFileUM>();
            CreateMap<RequestFileUM, RequestFile>();

            //Notification
            CreateMap<Notification, NotificationVM>();
            CreateMap<NotificationVM, Notification>();

            CreateMap<Notification, NotificationCM>();
            CreateMap<NotificationCM, Notification>();

            CreateMap<Notification, NotificationUM>();
            CreateMap<NotificationUM, Notification>();

            //UserNotification
            CreateMap<UserNotification, UserNotificationVM>();
            CreateMap<UserNotificationVM, UserNotification>();

            CreateMap<UserNotification, UserNotificationCM>();
            CreateMap<UserNotificationCM, UserNotification>();

            CreateMap<UserNotification, UserNotificationUM>();
            CreateMap<UserNotificationUM, UserNotification>();
        }
    }
}
