using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using System.Collections.Generic;

namespace Capstone.Service
{
    public interface IUserService
    {
        Dictionary<string, IEnumerable<string>> GetAuthorizationByUserID(string ID);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IGroupService _groupService;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository, IPermissionService permissionService, IRoleService roleService, IGroupService groupService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _permissionService = permissionService;
            _roleService = roleService;
            _groupService = groupService;
        }

        public Dictionary<string, IEnumerable<string>> GetAuthorizationByUserID(string ID)
        {
            Dictionary<string, IEnumerable<string>> data = new Dictionary<string, IEnumerable<string>>();
            IEnumerable<string> roles = _roleService.GetByUserID(ID);
            IEnumerable<string> groups = _groupService.GetByUserID(ID);
            IEnumerable<string> permissions = _permissionService.GetByUserID(ID);
            data.Add("role", roles);
            data.Add("group", groups);
            data.Add("permission", permissions);
            return data;
        }
    }
}
