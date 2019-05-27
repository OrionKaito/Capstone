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
        IEnumerable<Guid> GetByUserID(string ID);
        void Create(Permission permission);
        void Delete(Permission permission);
        void Save();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionOfRoleRepository _permissionOfRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IPermissionOfRoleRepository permissionOfRoleRepository,
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _permissionOfRoleRepository = permissionOfRoleRepository;
            _roleRepository = roleRepository;
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

        public IEnumerable<Guid> GetByUserID(string ID)
        {
            var permission = _permissionRepository.GetAll().Where(p => p.IsDelete == false);
            var por = _permissionOfRoleRepository.GetAll().Where(p => p.IsDelete == false);
            var role = _roleRepository.GetAll().Where(p => p.IsDelete == false);
            var ur = _userRoleRepository.GetAll().Where(p => p.IsDelete == false);

            var rs = permission.Join(por, p => p.ID, po => po.PermissionID, (p, po) => new
            {
                Names = p.Name,
                PermissId = p.ID,
                PermissName = p.Name,
                RoleId = po.RoleID
            }).Join(role, pn => pn.RoleId, r => r.ID, (pn, r) => new
            {
                RoleIds = r.ID,
                PermissIds = pn.PermissId,
                PermissNames = pn.PermissName
            }).Join(ur, rn => rn.RoleIds, u => u.RoleID, (rn, r) => new
            {
                RoleIds = r.ID,
                PermissIdss = rn.PermissIds,
                PermissNamess = rn.PermissNames,
                UserId = r.UserID
            }).Where(m => m.UserId.Equals(ID)).Select(m => m.PermissIdss);
            return rs;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
