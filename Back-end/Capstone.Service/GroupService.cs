using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IGroupService
    {
        IEnumerable<Group> GetAll();
        Group GetByID(Guid ID);
        Group GetByName(string Name);
        IEnumerable<string> GetByUserID(string ID);
        void Create(Group group);
        void Delete(Group group);
        void Save();
    }
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepository, IUserGroupRepository userGroupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Group group)
        {
            _groupRepository.Add(group);
        }

        public void Delete(Group group)
        {
            _groupRepository.Delete(group);
        }

        public IEnumerable<Group> GetAll()
        {
            return _groupRepository.GetAll().Where(g => g.IsDeleted == false);
        }

        public Group GetByID(Guid ID)
        {
            return _groupRepository.GetById(ID);
        }

        public Group GetByName(string Name)
        {
            return _groupRepository.GetByName(Name);
        }

        public IEnumerable<string> GetByUserID(string ID)
        {
            List<string> listGroupName = new List<string>();
            var data = _userGroupRepository.GetMany(u => u.IsDeleted == false && u.UserId.Equals(ID));
            foreach (var item in data)
            {
                listGroupName.Add(_groupRepository.GetById(item.GroupID).Name);
            }

            return listGroupName;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
