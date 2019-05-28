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
        IEnumerable<string> GetByUserID(string ID);
        void Create(Permission permission);
        void Delete(Permission permission);
        void Save();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionOfRoleRepository _permissionOfRoleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IPermissionOfRoleRepository permissionOfRoleRepository,
            IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _permissionOfRoleRepository = permissionOfRoleRepository;
            _userRoleRepository = userRoleRepository;
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

        public IEnumerable<string> GetByUserID(string ID)
        {
            List<UserRole> listUserRole = new List<UserRole>();
            var data = _userRoleRepository.GetMany(u => u.IsDelete == false && u.UserID.Equals(ID));
            foreach (var item in data)
            {
                listUserRole.Add(item);
            }

            List<PermissionOfRole> listPermissionOfRole = new List<PermissionOfRole>();
            foreach (var item in listUserRole)
            {
                var getByRoleID = _permissionOfRoleRepository.GetMany(p => p.IsDelete == false && p.RoleID == item.RoleID);
                foreach (var permissionItem in getByRoleID)
                {
                    listPermissionOfRole.Add(permissionItem);
                }
            }

            List<string> listNameOfPermission = new List<string>();
            foreach (var permission in listPermissionOfRole)
            {
                listNameOfPermission.Add(_permissionRepository.GetById(permission.PermissionID).Name);
            }
            
            return listNameOfPermission;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
