using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.Service
{
    public interface IUserGroupService
    {
        IEnumerable<UserGroup> GetByUserID(string ID);
        UserGroup GetByID(Guid ID);
        void Create(UserGroup userGroup);
        void Delete(UserGroup userGroup);
        void Save();
    }

    public class UserGroupService : IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserGroupService(IUserGroupRepository userGroupRepository, IUnitOfWork unitOfWork)
        {
            _userGroupRepository = userGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(UserGroup userGroup)
        {
            _userGroupRepository.Add(userGroup);
        }

        public void Delete(UserGroup userGroup)
        {
            _userGroupRepository.Delete(userGroup);
        }

        public IEnumerable<UserGroup> GetByUserID(string ID)
        {
            return _userGroupRepository.GetMany(u => u.IsDelete == false && u.UserId.Equals(ID));
        }

        public UserGroup GetByID(Guid ID)
        {
            return _userGroupRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
