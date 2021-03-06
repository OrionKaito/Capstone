﻿using Capstone.Data.Infrastructrure;
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
        IEnumerable<Permission> GetIsDelete();
        Permission GetByID(Guid ID);
        Permission GetByName(string Name);
        IEnumerable<Permission> GetByUserID(string ID);
        void Create(Permission permission);
        void Delete(Permission permission);
        void Save();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IPermissionOfGroupRepository _permissionOfGroupRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IPermissionOfGroupRepository permissionOfRoleRepository,
            IUserGroupRepository userGroupRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _permissionOfGroupRepository = permissionOfRoleRepository;
            _userGroupRepository = userGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Permission permission)
        {
            _permissionRepository.Add(permission);
            _unitOfWork.Commit();
        }

        public void Delete(Permission permission)
        {
            _permissionRepository.Delete(permission);
            _unitOfWork.Commit();
        }

        public IEnumerable<Permission> GetAll()
        {
            return _permissionRepository.GetAll();
        }

        public Permission GetByID(Guid ID)
        {
            return _permissionRepository.GetById(ID);
        }

        public Permission GetByName(string Name)
        {
            return _permissionRepository.GetByName(Name);
        }

        public IEnumerable<Permission> GetByUserID(string ID)
        {
            List<Permission> permissions = new List<Permission>();
            // lấy group của user
            var groups = _userGroupRepository.GetMany(u => u.IsDeleted == false && u.UserID.Equals(ID));
            foreach (var item in groups)
            {
                // lấy permission của group
                var permissionOfGroup = _permissionOfGroupRepository.GetMany(p => p.IsDeleted == false && p.GroupID == item.GroupID);
                foreach (var permissionItem in permissionOfGroup)
                {
                    permissions.Add(_permissionRepository.GetById(permissionItem.PermissionID));
                }
            }
            return permissions;
        }

        public IEnumerable<Permission> GetIsDelete()
        {
            return _permissionRepository.GetMany(p => p.IsDeleted == false);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
