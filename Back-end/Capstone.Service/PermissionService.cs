using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IPermissionService
    {
        IEnumerable<Permission> GetAll();
        Permission GetByID(Guid ID);
        void Create(Permission permission);
        void Delete(Permission permission);
        void Save();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Permission permission)
        {
            _permissionRepository.Add(permission);
        }

        public void Delete(Permission permission)
        {
            _permissionRepository.Delete(permission);
        }

        public IEnumerable<Permission> GetAll()
        {
            return _permissionRepository.GetAll().Where(p => p.IsDelete == false);
        }

        public Permission GetByID(Guid ID)
        {
            return _permissionRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
