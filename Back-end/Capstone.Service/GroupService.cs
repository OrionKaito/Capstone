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
            _unitOfWork.Commit();
        }

        public void Delete(Group group)
        {
            _groupRepository.Delete(group);
            _unitOfWork.Commit();
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

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
