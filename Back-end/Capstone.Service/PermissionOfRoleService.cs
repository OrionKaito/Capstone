using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IPermissionOfRoleService
    {
        IEnumerable<PermissionOfRole> GetAll();
        IEnumerable<PermissionOfRole> GetByRole(Guid ID);
        IEnumerable<PermissionOfRole> GetByPermission(Guid ID);
        PermissionOfRole GetByID(Guid ID);
        void Create(PermissionOfRole permissionOfRole);
        void Delete(PermissionOfRole permissionOfRole);
        void Save();
    }

    public class PermissionOfRoleService : IPermissionOfRoleService
    {
        private readonly IPermissionOfRoleRepository _permissionOfRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionOfRoleService(IPermissionOfRoleRepository permissionOfRoleRepository, IUnitOfWork unitOfWork)
        {
            _permissionOfRoleRepository = permissionOfRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(PermissionOfRole permissionOfRole)
        {
            _permissionOfRoleRepository.Add(permissionOfRole);
        }

        public void Delete(PermissionOfRole permissionOfRole)
        {
            _permissionOfRoleRepository.Delete(permissionOfRole);
        }

        public IEnumerable<PermissionOfRole> GetAll()
        {
            return _permissionOfRoleRepository.GetAll().Where(p => p.IsDelete == false);
        }

        public PermissionOfRole GetByID(Guid ID)
        {
            return _permissionOfRoleRepository.GetById(ID);
        }

        public IEnumerable<PermissionOfRole> GetByPermission(Guid ID)
        {
            return _permissionOfRoleRepository.GetMany(p => p.IsDelete == false && p.PermissionID == ID);
        }

        public IEnumerable<PermissionOfRole> GetByRole(Guid ID)
        {
            return _permissionOfRoleRepository.GetMany(p => p.IsDelete == false && p.RoleID == ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
