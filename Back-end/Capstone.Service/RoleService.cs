using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAll();
        Role GetByID(Guid ID);
        void Create(Role role);
        void Delete(Role role);
        void Save();
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(Role role)
        {
            _roleRepository.Add(role);
        }

        public void Delete(Role role)
        {
            _roleRepository.Delete(role);
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleRepository.GetAll().Where(r => r.IsDelete == false);
        }

        public Role GetByID(Guid ID)
        {
            return _roleRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
