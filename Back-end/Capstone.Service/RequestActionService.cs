using Capstone.Data.Infrastructrure;
using Capstone.Data.Repositories;
using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Service
{
    public interface IRequestActionService
    {
        IEnumerable<RequestAction> GetAll();
        IEnumerable<RequestAction> GetExceptActorID(string ID);
        RequestAction GetByID(Guid ID);
        RequestAction GetByActorID(string ID);
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

        public RequestAction GetByActorID(string ID)
        {
            return _requestActionRepository.GetMany(r => r.ActorID.Equals(ID)).FirstOrDefault();
        }

        public IEnumerable<RequestAction> GetExceptActorID(string ID)
        {
            return _requestActionRepository.GetMany(r => !r.ActorID.Equals(ID));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
