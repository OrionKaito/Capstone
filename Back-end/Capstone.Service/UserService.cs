using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IUserService
    {
        Dictionary<string, IEnumerable<string>> GetAuthorizationByUserID(string ID);
        IEnumerable<User> getUsersByPermissionID(Guid ID);
        void BeginTransaction();
        void CommitTransaction();
        void RollBack();
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IPermissionOfGroupRepository _permissionOfGroupRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        //Sửa lại chỗ này nha Thanh Lộc
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;
        private readonly IGroupService _groupService;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository
            , IPermissionOfGroupRepository permissionOfGroupRepository
            , IUserGroupRepository userGroupRepository
            , IPermissionService permissionService
            , IRoleService roleService
            , IGroupService groupService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _permissionOfGroupRepository = permissionOfGroupRepository;
            _userGroupRepository = userGroupRepository;
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

        public IEnumerable<User> getUsersByPermissionID(Guid ID)
        {
            List<User> users = new List<User>();
            var groups = _permissionOfGroupRepository.GetMany(r => r.PermissionID == ID)
                //.ToList()
                .Select(g => new Role { ID = g.GroupID })
                .ToList();

            foreach (var group in groups)
            {
                var result = _userGroupRepository.GetMany(u => u.GroupID == group.ID).Select(u => new User { Id = u.UserID }).ToList();
                foreach (var data in result)
                {
                    users.Add(data);
                }
            }

            return users;
        }

        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }
        public void CommitTransaction()
        {
            _unitOfWork.CommitTransaction();
        }

        public void RollBack()
        {
            _unitOfWork.RollBack();
        }
    }
}
