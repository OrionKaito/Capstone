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
        void Create(Group group);
        void Delete(Group group);
        void Save();
    }
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IGroupRepository groupRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
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
            return _groupRepository.GetAll().Where(g => g.IsDelete == false);
        }

        public Group GetByID(Guid ID)
        {
            return _groupRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
