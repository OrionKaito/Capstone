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

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository
            , IPermissionOfGroupRepository permissionOfGroupRepository
            , IUserGroupRepository userGroupRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _permissionOfGroupRepository = permissionOfGroupRepository;
            _userGroupRepository = userGroupRepository;
        }

        public IEnumerable<User> getUsersByPermissionID(Guid ID)
        {
            List<User> users = new List<User>();
            var groups = _permissionOfGroupRepository.GetMany(r => r.PermissionID == ID)
                //.ToList()
                .Select(g => new Group { ID = g.GroupID })
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
