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

            CreateMap<ActionType, ActionTypeVM>();
            CreateMap<ActionTypeVM, ActionType>();

            CreateMap<ActionType, ActionTypeCM>();
            CreateMap<ActionTypeCM, ActionType>();

            CreateMap<ActionType, ActionTypeUM>();
            CreateMap<ActionTypeUM, ActionType>();

            CreateMap<Group, GroupVM>();
            CreateMap<GroupVM, Group>();

            CreateMap<Group, GroupCM>();
            CreateMap<GroupCM, Group>();

            CreateMap<Group, GroupUM>();
            CreateMap<GroupUM, Group>();
        }
    }
}
