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
        Role GetByName(string Name);
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
            _unitOfWork.Commit();
        }

        public void Delete(Role role)
        {
            _roleRepository.Delete(role);
            _unitOfWork.Commit();
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleRepository.GetAll().Where(r => r.IsDeleted == false);
        }

        public Role GetByID(Guid ID)
        {
            return _roleRepository.GetById(ID);
        }

        public Role GetByName(string Name)
        {
            return _roleRepository.GetByName(Name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
