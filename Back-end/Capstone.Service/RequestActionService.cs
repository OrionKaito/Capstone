using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Service
{
    public interface IRequestActionService
    {
        IEnumerable<RequestAction> GetAll();
        RequestAction GetByID(Guid ID);
        void Create(RequestAction requestAction);
        void Save();
    }
    public class RequestActionService : IRequestActionService
    {
        private readonly IRequestActionRepository _requestActionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequestActionService(IRequestActionRepository requestActionRepository, IUnitOfWork unitOfWork)
        {
            _requestActionRepository = requestActionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(RequestAction requestAction)
        {
            _requestActionRepository.Add(requestAction);
            _unitOfWork.Commit();
        }

        public IEnumerable<RequestAction> GetAll()
        {
            return _requestActionRepository.GetAll();
        }

        public RequestAction GetByID(Guid ID)
        {
            return _requestActionRepository.GetById(ID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
