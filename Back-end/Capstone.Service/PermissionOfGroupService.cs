using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IPermissionOfGroupService
    {
        IEnumerable<PermissionOfGroup> GetAll();
        IEnumerable<PermissionOfGroup> GetByGroupID(Guid ID);
        IEnumerable<PermissionOfGroup> GetByPermission(Guid ID);
        PermissionOfGroup GetByID(Guid ID);
        PermissionOfGroup CheckExist(Guid PermissionID, Guid GroupID);
        PermissionOfGroup GetByGroupIDAndPermissionID(Guid GroupID, Guid PermissionID);
        void Create(PermissionOfGroup permissionOfGroup);
        void Delete(PermissionOfGroup permissionOfGroup);
        void Save();
    }

    public class PermissionOfGroupService : IPermissionOfGroupService
    {
        private readonly IPermissionOfGroupRepository _permissionOfGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionOfGroupService(IPermissionOfGroupRepository permissionOfGroupRepository, IUnitOfWork unitOfWork)
        {
            _permissionOfGroupRepository = permissionOfGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public PermissionOfGroup CheckExist(Guid PermissionID, Guid GroupID)
        {
            return _permissionOfGroupRepository.CheckExist(PermissionID, GroupID);
        }

        public void Create(PermissionOfGroup permissionOfGroup)
        {
            _permissionOfGroupRepository.Add(permissionOfGroup);
            _unitOfWork.Commit();
        }

        public void Delete(PermissionOfGroup permissionOfGroup)
        {
            _permissionOfGroupRepository.Delete(permissionOfGroup);
            _unitOfWork.Commit();
        }

        public IEnumerable<PermissionOfGroup> GetAll()
        {
            return _permissionOfGroupRepository.GetAll().Where(p => p.IsDeleted == false);
        }

        public PermissionOfGroup GetByID(Guid ID)
        {
            return _permissionOfGroupRepository.GetById(ID);
        }

        public IEnumerable<PermissionOfGroup> GetByPermission(Guid ID)
        {
            return _permissionOfGroupRepository.GetMany(p => p.IsDeleted == false && p.PermissionID == ID);
        }

        public IEnumerable<PermissionOfGroup> GetByGroupID(Guid ID)
        {
            return _permissionOfGroupRepository.GetMany(p => p.IsDeleted == false && p.GroupID == ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public PermissionOfGroup GetByGroupIDAndPermissionID(Guid GroupID, Guid PermissionID)
        {
            return _permissionOfGroupRepository.GetMany(p => p.GroupID == GroupID && p.PermissionID == PermissionID).FirstOrDefault();
        }
    }
}
