using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;

namespace Capstone.Service
{
    public interface IRequestActionService
    {
        IEnumerable<RequestAction> GetAll();
        IEnumerable<RequestAction> GetExceptStartAction(Guid startActionTemplateID, Guid RequestID);
        IEnumerable<RequestAction> GetActionByStatus(StatusEnum statusEnum);
        RequestAction GetByID(Guid ID);
        RequestAction GetStartAction(Guid startActionTemplateID, Guid requestID);
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

        public RequestAction GetStartAction(Guid startActionTemplateID, Guid requestID)
        {
            return _requestActionRepository.GetStartAction(startActionTemplateID, requestID);
        }

        public IEnumerable<RequestAction> GetExceptStartAction(Guid startActionTemplateID, Guid requestID)
        {
            return _requestActionRepository.GetExceptStartAction(startActionTemplateID, requestID);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<RequestAction> GetActionByStatus(StatusEnum statusEnum)
        {
            return _requestActionRepository.GetActionByStatus(statusEnum);
        }
    }
}
