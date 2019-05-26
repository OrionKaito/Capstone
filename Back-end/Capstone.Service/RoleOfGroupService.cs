using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IRoleOfGroupService
    {
        IEnumerable<RoleOfGroup> GetAll();
        IEnumerable<RoleOfGroup> GetByGroup(Guid ID);
        IEnumerable<RoleOfGroup> GetByRole(Guid ID);
        RoleOfGroup GetByID(Guid ID);
        void Create(RoleOfGroup rog);
        void Delete(RoleOfGroup rog);
        void Save();
    }

    public class RoleOfGroupService : IRoleOfGroupService
    {
        private readonly IRoleOfGroupRepository _roleOfGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleOfGroupService(IRoleOfGroupRepository roleOfGroupRepository, IUnitOfWork unitOfWork)
        {
            _roleOfGroupRepository = roleOfGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(RoleOfGroup rog)
        {
            _roleOfGroupRepository.Add(rog);
        }

        public void Delete(RoleOfGroup rog)
        {
            _roleOfGroupRepository.Delete(rog);
        }

        public IEnumerable<RoleOfGroup> GetAll()
        {
            return _roleOfGroupRepository.GetAll().Where(r => r.IsDelete == false);
        }

        public IEnumerable<RoleOfGroup> GetByGroup(Guid ID)
        {
            return _roleOfGroupRepository.GetMany(r => r.IsDelete == false && r.GroupID == ID);
        }

        public IEnumerable<RoleOfGroup> GetByRole(Guid ID)
        {
            return _roleOfGroupRepository.GetMany(r => r.IsDelete == false && r.RoleID == ID);
        }

        public RoleOfGroup GetByID(Guid ID)
        {
            return _roleOfGroupRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
