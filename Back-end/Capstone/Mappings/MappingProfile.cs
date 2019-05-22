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
        }
    }
}
