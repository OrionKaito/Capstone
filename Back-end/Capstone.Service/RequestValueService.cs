using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Service
{
    public interface IRequestValueService
    {
        IEnumerable<RequestValue> GetAll();
        RequestValue GetByID(Guid ID);
        void Create(RequestValue requestValue);
        void Delete(RequestValue requestValue);
        void Save();
    }
    public class RequestValueService : IRequestValueService
    {
        private readonly IRequestValueRepository _requestValueRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestValueService(IRequestValueRepository requestValueRepository, IUnitOfWork unitOfWork)
        {
            _requestValueRepository = requestValueRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(RequestValue requestValue)
        {
            _requestValueRepository.Add(requestValue);
        }

        public void Delete(RequestValue requestValue)
        {
            _requestValueRepository.Delete(requestValue);
        }

        public IEnumerable<RequestValue> GetAll()
        {
            return _requestValueRepository.GetAll();
        }

        public RequestValue GetByID(Guid ID)
        {
            return _requestValueRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
