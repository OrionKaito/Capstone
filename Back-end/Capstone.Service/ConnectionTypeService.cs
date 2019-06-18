using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Service
{
    public interface IConnectionTypeService
    {
        IEnumerable<ConnectionType> GetAll();
        ConnectionType GetByID(Guid ID);
        ConnectionType GetByName(string name);
        void Create(ConnectionType connectionType);
        void Save();
    }

    public class ConnectionTypeService : IConnectionTypeService
    {
        private readonly IConnectionTypeRepository _connectionTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConnectionTypeService(IConnectionTypeRepository connectionTypeRepository, IUnitOfWork unitOfWork)
        {
            _connectionTypeRepository = connectionTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(ConnectionType connectionType)
        {
            _connectionTypeRepository.Add(connectionType);
            _unitOfWork.Commit();
        }

        public IEnumerable<ConnectionType> GetAll()
        {
            return _connectionTypeRepository.GetAll().Where(c => c.IsDeleted == false);
        }

        public ConnectionType GetByID(Guid ID)
        {
            return _connectionTypeRepository.GetById(ID);
        }

        public ConnectionType GetByName(string name)
        {
            return _connectionTypeRepository.GetByName(name);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
