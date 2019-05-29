using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Service
{
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetAll();
        IEnumerable<UserRole> GetByUserID(string ID);
        UserRole GetByID(Guid ID);
        UserRole CheckExist(string UserID, Guid RoleID);
        void Create(UserRole userRole);
        void Delete(UserRole userRole);
        void Save();
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public UserRole CheckExist(string UserID, Guid RoleID)
        {
            return _userRoleRepository.CheckExist(UserID, RoleID);
        }

        public void Create(UserRole userRole)
        {
            _userRoleRepository.Add(userRole);
            _unitOfWork.Commit();
        }

        public void Delete(UserRole userRole)
        {
            _userRoleRepository.Delete(userRole);
            _unitOfWork.Commit();
        }

        public IEnumerable<UserRole> GetAll()
        {
            return _userRoleRepository.GetAll().Where(u => u.IsDeleted == false);
        }

        public UserRole GetByID(Guid ID)
        {
            return _userRoleRepository.GetById(ID);
        }

        public IEnumerable<UserRole> GetByUserID(string ID)
        {
            return _userRoleRepository.GetMany(u => u.IsDeleted == false && u.UserID.Equals(ID));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
